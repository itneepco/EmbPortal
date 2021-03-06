using Api.Models;
using Application.CQRS.Uoms.Query;
using Application.Uoms.Command;
using Application.Uoms.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    public class UomController : ApiController
    {
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<UomResponse>>> GetAllUoms()
        {
            return Ok(await Mediator.Send(new GetUomsQuery()));
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserResponse>>> GetUomsByPagination([FromQuery] PagedRequest request)
        {
            var query = new GetUomsWithPaginationQuery(request);
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("dimension")]
        public async Task<ActionResult<IReadOnlyList<UomDimensionResponse>>> GetUomDimensions()
        {
            return Ok(await Mediator.Send(new GetUomDimensionsQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UomResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UomResponse>> GetUomById(int id)
        {
            return Ok(await Mediator.Send(new GetUomByIdQuery(id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateUom(UomRequest request)
        {
            var command = new CreateUomCommand(request);

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUom(int id, UomRequest request)
        {
            var command = new EditUomCommand(id: id, name: request.Name, dimension: request.Dimension);
            await Mediator.Send(command);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUom(int id)
        {
            var command = new DeleteUomCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
