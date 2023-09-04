using Api.Models;
using Application.CQRS.WorkOrders.Command;
using Application.CQRS.WorkOrders.Query;
using Application.WorkOrders.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using EmbPortal.Shared.Requests.MeasurementBooks;
using Domain.Entities.WorkOrderAggregate;

namespace Api.Controllers;

[Authorize(Roles = "Admin, Manager")]
public class WorkOrderController : ApiController
{
    private readonly IConfiguration _config;
    private readonly ILogger<WorkOrderController> _logger;
    public WorkOrderController(IConfiguration config, ILogger<WorkOrderController> logger)
    {
        _config = config;
        _logger = logger;
    }

    [HttpGet("self")]
    public async Task<ActionResult<PaginatedList<WorkOrderResponse>>> GetWorkOrdersByCreator([FromQuery] PagedRequest request)
    {
        var query = new GetOrdersByUserPaginationQuery(request);

        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkOrderDetailResponse>> GetWorkOrderById(int id)
    {
        var query = new GetWorkOrderByIdQuery(id);

        return Ok(await Mediator.Send(query));
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateWorkOrder(WorkOrderRequest data)
    {
        var purchaseOrder = await FetchPODetailsFromSAP(data.OrderNo);

        if (purchaseOrder == null)
        {
            return NotFound(new ApiResponse(404, "Unable to fetch data from SAP"));
        }

        var command = new CreateWorkOrderCommand(purchaseOrder);

        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWorkOrde(int id)
    {
        var command = new DeleteWorkOrderCommand(id);
        await Mediator.Send(command);

        return NoContent();
    }

    
    [HttpPut("{workOrderId}/Transfer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> TransferWorkOrder(int workOrderId, ChangeOfficerRequest data)
    {
        var command = new ChangeEngineerInChargeCommand(workOrderId, data);
        await Mediator.Send(command);

        return NoContent();
    }

    
    [HttpGet("{workOrderId}/Item/Pending")]
    public async Task<ActionResult<IReadOnlyList<PendingOrderItemResponse>>> GetPendingWorkOrderItems(int workOrderId)
    {
        var query = new GetPendingWorkOrderItemsQuery(workOrderId);

        return Ok(await Mediator.Send(query));
    }


    [HttpGet("sap/{purchaseOrderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(PurchaseOrder), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderFromSAP(long purchaseOrderId)
    {
        var purchaseOrder = await FetchPODetailsFromSAP(purchaseOrderId);

        if (purchaseOrder == null)
        {
            return NotFound(new ApiResponse(404, "Unable to fetch data from SAP"));
        }

        return Ok(purchaseOrder);
    }

    [HttpGet("sap/refetch/{poNo}")]    
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> RefetchPOFromSAP(long poNo)
    {
        var purchaseOrder = await FetchPODetailsFromSAP(poNo);

        if (purchaseOrder == null)
        {
            return NotFound(new ApiResponse(404, "Unable to fetch data from SAP"));
        }

        var command = new UpdateWorkOrderCommand(purchaseOrder);        
        return Ok(await Mediator.Send(command));
    }

    private async Task<PurchaseOrder> FetchPODetailsFromSAP(long purchaseOrderId)
    {
        try
        {
            var url = $"{_config["POUrl"]}/{purchaseOrderId}";
            var authToken = Encoding.ASCII.GetBytes($"{_config["UserId"]}:{_config["Password"]}");
            
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(authToken));

            var response = await httpClient.GetAsync(url);

            var responseAsString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var purchaseOrder = JsonSerializer.Deserialize<PurchaseOrder>(responseAsString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return purchaseOrder;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
        }

        return null;
    }
}
