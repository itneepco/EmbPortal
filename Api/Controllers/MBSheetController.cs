using Api.Models;
using Application.CQRS.MBSheets.Command;
using Application.CQRS.MBSheets.Query;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class MBSheetController : ApiController
    {
        [HttpGet("MBook/{mBookId}")]
        public async Task<ActionResult<List<MBSheetResponse>>> GetMBooksByOrderId(int mBookId)
        {
            var query = new GetMBSheetsByMBookIdQuery(mBookId);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateMBSheet(MBSheetRequest data)
        {
            var command = new CreateMBSheetCommand(data);

            return Ok(await Mediator.Send(command));
        }
    }
}
