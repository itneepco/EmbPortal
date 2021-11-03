using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Projects.Query;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<ProjectResponse>> Getproject(int id) 
        {
            return Ok(await Mediator.Send(new GetProjectByIdQuery{Id = id}));
        }
    }
}