using Api.Models;
using Application.Identity.Commands;
using Application.Identity.Commands.RegisterUser;
using Application.Identity.Commands.UpdateUser;
using Application.Identity.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmbPortal.Shared.Extensions;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmbPortal.Shared.Requests;
using Application.CQRS.Identity.Queries;

namespace Api.Controllers
{
    public class IdentityController : ApiController
    {
        [HttpGet("Emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return Ok(await Mediator.Send(new CheckEmailExistsQuery { Email = email }));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthUserResponse>> Login(LoginRequest request)
        {
            var command = new LoginUserCommand(request);
            return Ok(await Mediator.Send(command));
        }

        [Authorize]
        [HttpGet("Currentuser")]
        public async Task<ActionResult<AuthUserResponse>> GetCurrentUser()
        {
            var email = HttpContext.User.GetEmailFromClaimsPrincipal();

            return Ok(await Mediator.Send(new GetCurrentUserQuery(email)));
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<PaginatedList<UserResponse>>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<UserResponse>>> GetUsers([FromQuery] PagedRequest request)
        {
            var query = new GetUsersWithPaginationQuery(request);
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}/Roles")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetUserRoles(string userId)
        {
            return Ok(await Mediator.Send(new GetUserRolesQuery(userId)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}/Reset")]
        public async Task<ActionResult> ResetUserPassword(string userId)
        {
            return Ok(await Mediator.Send(new GetUserRolesQuery(userId)));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Roles")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetAllRoles()
        {
            return Ok(await Mediator.Send(new GetRolesQuery()));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(UserRequest request)
        {
            if (CheckEmailExistsAsync(request.Email).Result.Value)
            {
                var errors = new List<string>();
                errors.Add("Email address is in use");

                var response = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(response);
            }

            var command = new RegisterUserCommand(request);
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}")]
        public async Task<ActionResult<string>> UpdateUser(string userId, UserRequest request)
        {
            var command = new UpdateUserCommand(userId, request);
            return Ok(await Mediator.Send(command));
        }
    }
}
