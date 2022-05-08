using Api.Models;
using Application.CQRS.MBSheets.Command;
using Application.CQRS.MBSheets.Query;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    public class MBSheetController : ApiController
    {
        private readonly IWebHostEnvironment env;

        public MBSheetController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpGet("MBook/{mBookId}")]
        public async Task<ActionResult<List<MBSheetResponse>>> GetMBSheetsByMBookId(int mBookId)
        {
            var query = new GetMBSheetsByMBookIdQuery(mBookId);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{mBSheetId}")]
        [ProducesResponseType(typeof(MBSheetResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MBSheetResponse>> GetMBSheetsById(int mBSheetId)
        {
            var query = new GetMBSheetByIdQuery(mBSheetId);

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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EditMBSheet(MBSheetRequest data, int id)
        {
            var command = new EditMBSheetCommand(data, id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Validate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ValidateMBSheet(int id)
        {
            var command = new ValidateMBSheetCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("{id}/Accept")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ApproveMBSheet(int id)
        {
            var command = new AcceptMBSheetCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMBSheet(int id)
        {
            var command = new DeleteMBSheetCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{mbSheetId}/Item")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateMBSheetItem(int mbSheetId, MBSheetItemRequest data)
        {
            var command = new CreateMBSheetItemCommand(mbSheetId, data);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{mbSheetId}/Item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateMBSheetItem(int mbSheetId, int itemId, MBSheetItemRequest data)
        {
            var command = new EditMBSheetItemCommand(itemId, mbSheetId, data);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{mbSheetId}/Item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMBSheetItem(int mbSheetId, int itemId)
        {
            var command = new DeleteMBSheetItemCommand(itemId, mbSheetId);
            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPost("{mbSheetId}/Item/{itemId}/Uploads")]
        [ProducesResponseType(typeof(IList<UploadResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(int mbSheetId, int itemId, [FromForm] IEnumerable<IFormFile> files)
        {
            var command = new UploadMBSheetAttachmentsCommand(files, env.ContentRootPath);

            return Ok(await Mediator.Send(command));
        }
    }
}
