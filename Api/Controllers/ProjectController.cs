using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Application.Projects.Command;
using Application.Projects.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests;
using Shared.Responses;

namespace Api.Controllers
{
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

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateProject(ProjectRequest request)
        {
            CreateMeasurementBookCommand command = new CreateMeasurementBookCommand(name: request.Name);

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProject(int id, ProjectRequest request)
        {
            EditProjectCommand command = new EditProjectCommand(id: id, name: request.Name);
            await Mediator.Send(command);

            return NoContent();
        }

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