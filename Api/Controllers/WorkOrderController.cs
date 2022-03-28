using Api.Models;
using Application.CQRS.WorkOrders.Command;
using Application.CQRS.WorkOrders.Query;
using Application.WorkOrders.Command;
using Application.WorkOrders.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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
            var command = new CreateWorkOrderCommand(data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateWorkOrder(int id, WorkOrderRequest data)
        {
            var command = new EditWorkOrderCommand(id, data);
            await Mediator.Send(command);

            return NoContent();
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

        [HttpPut("{workOrderId}/Publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PublishWorkOrder(int workOrderId)
        {
            var command = new PublishWorkOrderCommand(workOrderId);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{workOrderId}/Items/{orderItemId}/Publish")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PublishWorkOrderItem(int workOrderId, int orderItemId)
        {
            var command = new PublishWorkOrderItemCommand(workOrderId, orderItemId);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpGet("{workOrderId}/Item/Pending")]
        public async Task<ActionResult<IReadOnlyList<PendingOrderItemResponse>>> GetPendingWorkOrderItems(int workOrderId)
        {
            var query = new GetPendingWorkOrderItemsQuery(workOrderId);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost("{workOrderId}/Item")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateWorkOrderItem(int workOrderId, WorkOrderItemRequest data)
        {
            var command = new CreateWorkOrderItemCommand(workOrderId, data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{workOrderId}/Item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateWorkOrderItem(int workOrderId, int itemId, WorkOrderItemRequest data)
        {
            var command = new EditWorkOrderItemCommand(itemId, workOrderId, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{workOrderId}/Item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteWorkOrderItem(int workOrderId, int itemId)
        {
            var command = new DeleteWorkOrderItemCommand(itemId, workOrderId);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
