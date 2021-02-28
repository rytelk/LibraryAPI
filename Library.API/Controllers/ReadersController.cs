using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Library.Application.Commands.Readers.CreateReaderCommand;
using Library.Application.Commands.Readers.DeleteReaderCommand;
using Library.Application.Commands.Readers.UpdateReaderCommand;
using Library.Application.DTOs;
using Library.Application.Queries.Readers.GetReaderDetailsQuery;
using Library.Application.Queries.Readers.GetReaderListQuery;
using Library.Domain.AggregatesModel.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Library.API.Controllers
{
    [Route("api/readers")]
    [ApiController]
    [Authorize(Roles = UserRolesConsts.Librarian)]
    public class ReadersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReadersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        ///     Get reader details
        /// </summary>
        /// <param name="userId">User id</param>
        /// <response code="200">User details</response>
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> GetReaderDetails(int userId)
        {
            var user = await _mediator.Send(new GetReaderDetailsQuery()
            {
                UserId = userId,
            });
            return Ok(user);
        }

        /// <summary>
        ///     Get reader list
        /// </summary>
        /// <response code="200">Readers list</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDTO>>> GetReaders()
        {
            var users = await _mediator.Send(new GetReaderListQuery());
            return Ok(users);
        }


        /// <summary>
        ///     Create a reader
        /// </summary>
        /// <param name="command">Reader data</param>
        /// <response code="200">Created reader details</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> CreateReader(CreateReaderCommand command)
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        /// <summary>
        ///     Update a reader
        /// </summary>
        /// <param name="command">User data</param>
        /// <response code="200">Updated user details</response>
        [HttpPut]
        [ProducesResponseType(typeof(UserDTO) ,StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> UpdateReader(UpdateReaderCommand command)
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        /// <summary>
        ///     Delete a reader
        /// </summary>
        /// <param name="userId">User id</param>
        /// <response code="204"></response>
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteReader(int userId)
        {
            await _mediator.Send(new DeleteReaderCommand()
            {
                UserId = userId
            });
            return NoContent();
        }

    }
}
