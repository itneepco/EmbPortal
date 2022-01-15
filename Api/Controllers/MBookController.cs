using Api.Models;
using Application.CQRS.MeasurementBooks.Command;
using Application.CQRS.MeasurementBooks.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class MBookController : ApiController
    {
        [HttpGet("WorkOrder/{orderId}")]
        public async Task<ActionResult<PaginatedList<MeasurementBookResponse>>> GetMBooksByOrderId(int orderId)
        {
            var query = new GetMBooksByOrderIdQuery(orderId);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("CurrentUser")]
        public async Task<ActionResult<PaginatedList<MeasurementBookResponse>>> GetMBooksByUserId([FromQuery] PagedRequest request)
        {
            var query = new GetMBooksByUserIdPaginationQuery(request);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MBookDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MBookDetailResponse>> GetMBookById(int id)
        {
            return Ok(await Mediator.Send(new GetMBookByIdQuery(id)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateMeasurementBook(MBookRequest data)
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMeasurementBook(int id)
        {
            var command = new DeleteMBookCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
