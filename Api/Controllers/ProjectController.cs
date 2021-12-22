using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Application.Projects.Command;
using Application.Projects.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Authorize]
    public class ProjectController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProjectResponse>>> GetProjects()
        {
            return Ok(await Mediator.Send(new GetProjectsQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectResponse>> GetProjectById(int id) 
        {
            return Ok(await Mediator.Send(new GetProjectByIdQuery(id)));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateProject(ProjectRequest request)
        {
            var command = new CreateMeasurementBookCommand(name: request.Name);

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProject(int id, ProjectRequest request)
        {
            var command = new EditProjectCommand(id: id, name: request.Name);
            await Mediator.Send(command);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProjectCommand(id);
            await Mediator.Send(command);

            return NoContent();
        }
    }
}