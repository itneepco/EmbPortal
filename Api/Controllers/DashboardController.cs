using Application.CQRS.Dashboard.Queries;
using EmbPortal.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpGet("Stats")]
        public async Task<ActionResult<List<MBookResponse>>> GetDashboardStats()
        {
            var query = new DashboardStatsQuery();

            return Ok(await Mediator.Send(query));
        }
    }
}
