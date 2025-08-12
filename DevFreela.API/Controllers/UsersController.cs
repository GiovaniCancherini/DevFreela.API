using DevFreela.Application.Commands.InsertProfilePictureInUser;
using DevFreela.Application.Commands.InsertSkillsInUser;
using DevFreela.Application.Commands.InsertUser;
using DevFreela.Application.Commands.LoginUser;
using DevFreela.Application.Queries.GetAllUsers;
using DevFreela.Application.Queries.GetUserById;
using DevFreela.Core.Enums;
using DevFreela.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _auth;

        public UsersController(IMediator mediator, IAuthService auth)
        {
            _mediator = mediator;
            _auth = auth;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRole.Client}, {UserRole.Freelancer}")]
        public async Task<IActionResult> Get(string search = "")
        {
            var query = new GetAllUsersQuery(search);

            var result = await _mediator.Send(query);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRole.Client}, {UserRole.Freelancer}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserByIdQuery(id);

            var result = await _mediator.Send(query);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRole.Client}")]
        public async Task<IActionResult> Post(InsertUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
        }

        [HttpPost("{id}/skills")]
        [Authorize(Roles = $"{UserRole.Client}, {UserRole.Freelancer}")]
        public async Task<IActionResult> PostSkills(int id, InsertSkillsInUserCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("{id}/profile-picture")]
        [Authorize(Roles = $"{UserRole.Freelancer}")]
        public async Task<IActionResult> PostProfilePicture(int id, InsertProfilePictureInUserCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(int id, LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSucess)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}