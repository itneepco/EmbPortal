using Api.Models;
using Application.CQRS.RABills.Commands;
using Application.CQRS.RABills.Queries;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    public class RABillController : ApiController
    {
        [HttpGet("MBook/{mBookId}")]
        public async Task<ActionResult<List<RABillResponse>>> GetRABillsByMBookId(int mBookId)
        {
            var query = new GetRABillByMBookIdQuery(mBookId);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{raBillId}")]
        [ProducesResponseType(typeof(RABillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RABillResponse>> GetRABillById(int raBillId)
        {
            var query = new GetRABillByIdQuery(raBillId);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateRABill(RABillRequest data)
        {
            var command = new CreateRABillCommand(data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditRABill(RABillRequest data, int id)
        {
            var command = new EditRABillCommand(id, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ApproveRABill(int id)
        {
            var command = new ApproveRABillCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RevokeRABill(int id)
        {
            var command = new RevokeRABillCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteRABill(int id)
        {
            var command = new DeleteRABillCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
