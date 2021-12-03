using Api.Models;
using Application.CQRS.MeasurementBooks.Command;
using Application.CQRS.MeasurementBooks.Query;
using Application.CQRS.WorkOrders.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using Shared.Responses;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class MBookController : ApiController
    {
        [HttpGet("WorkOrder/{orderId}")]
        public async Task<ActionResult<PaginatedList<MeasurementBookResponse>>> GetMBookByOrderId(int orderId, [FromQuery] PagedRequest request)
        {
            var query = new GetMBByOrderIdPaginationQuery(orderId, request);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateMeasurementBook(CreateMBookRequest data)
        {
            var command = new CreateMBookCommand(data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMeasurementBook(int id, MBookRequest data)
        {
            var command = new EditMBookCommand(id, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{mBookId}/Item")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateMeasurementBookItem(int mBookId, MBookItemRequest data)
        {
            var command = new CreateMBItemCommand(mBookId, data.WorkOrderItemId);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{mBookId}/Item/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateWorkOrderItem(int mBookId, int id, MBookItemRequest data)
        {
            var command = new EditMBItemCommand(id, mBookId, data.WorkOrderItemId);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{mBookId}/Item/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteWorkOrderItem(int id, int mBookId)
        {
            var command = new DeleteMBItemCommand(id, mBookId);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
