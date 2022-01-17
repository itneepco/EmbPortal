using Api.Models;
using Application.CQRS.MBSheets.Command;
using EmbPortal.Shared.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class MBSheetController : ApiController
    {
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateMeasurementBook(MBSheetRequest data)
        {
            var command = new CreateMBSheetCommand(data);

            return Ok(await Mediator.Send(command));
        }
    }
}
