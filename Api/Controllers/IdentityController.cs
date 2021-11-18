using Api.Models;
using Application.Identity.Commands;
using Application.Identity.Commands.RegisterUser;
using Application.Identity.Commands.UpdateUser;
using Application.Identity.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using Shared.Identity;
using Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class IdentityController : ApiController
    {
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserDto>>> GetUsers([FromQuery] GetUsersWithPaginationQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<ActionResult<AuthUserDto>> GetCurrentUser()
        {
            var email = HttpContext.User.GetEmailFromClaimsPrincipal();

            return await Mediator.Send(new GetCurrentUserQuery { Email = email });
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await Mediator.Send(new CheckEmailExistsQuery { Email = email });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthUserDto>> Login(LoginUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterUserCommand command)
        {
            if (CheckEmailExistsAsync(command.Email).Result.Value)
            {
                var errors = new List<string>();
                errors.Add("Email address is in use");

                var response = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(response);
            }

            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateuser")]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
