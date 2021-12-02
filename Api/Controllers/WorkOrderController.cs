﻿using Api.Models;
using Application.CQRS.WorkOrders.Command;
using Application.CQRS.WorkOrders.Query;
using Application.WorkOrders.Command;
using Application.WorkOrders.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using Shared.Responses;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class WorkOrderController : ApiController
    {
        [HttpGet("self")]
        public async Task<ActionResult<PaginatedList<WorkOrderResponse>>> GetWorkOrdersByCreator([FromQuery] PagedRequest request)
        {
            var query = new GetOrdersByCreatorPaginationQuery(request);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("Project/{projectId}")]
        public async Task<ActionResult<PaginatedList<WorkOrderResponse>>> GetWorkOrdersByProjects(int projectId, [FromQuery] PagedRequest request)
        {
            var query = new GetOrdersByProjectPaginationQuery(projectId, request);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateWorkOrder(CreateWorkOrderRequest data)
        {
            CreateWorkOrderCommand command = new CreateWorkOrderCommand(data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateWorkOrder(int id, WorkOrderRequest data)
        {
            EditWorkOrderCommand command = new EditWorkOrderCommand(id, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{workOrderId}/Item")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateWorkOrderItem(int workOrderId, WorkOrderItemRequest data)
        {
            CreateWorkOrderItemCommand command = new CreateWorkOrderItemCommand(workOrderId, data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{workOrderId}/Item/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateWorkOrderItem(int workOrderId, int id, WorkOrderItemRequest data)
        {
            EditWorkOrderItemCommand command = new EditWorkOrderItemCommand(id, workOrderId, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{workOrderId}/Item/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteWorkOrderItem(int id, int workOrderId)
        {
            var command = new DeleteWorkOrderItemCommand(id, workOrderId);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}