using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Library.Application.Commands.Accounts.LoginCommand;
using Library.Application.Commands.Accounts.RegisterCommand;
using Library.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Library.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Login
        /// </summary>
        /// <param name="command">User credentials</param>
        /// <response code="200">JWT token</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Login(LoginCommand command)
        {
            var jwtToken = await _mediator.Send(command);
            return Ok(jwtToken);
        }

        /// <summary>
        ///     Create a new user
        /// </summary>
        /// <param name="command">User data</param>
        /// <response code="200">Created user data</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> Register(RegisterCommand command)
        {
            var userDto = await _mediator.Send(command);
            return Ok(userDto);
        }
    }
}
