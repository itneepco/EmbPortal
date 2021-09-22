using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Projects.Query;
using Application.Projects.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
   public class ProjectController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProjectDto>>> GetProjects()
        {
            return Ok(await Mediator.Send(new GetProjectsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> Getproject(int id) {
            return Ok(await Mediator.Send(new GetProjectByIdQuery()));
        }
    }
}