using Arkano.Transactions.Aplication.Transactions.Queries;
using Arkano.Transactions.Aplication.Dtos;
using Arkano.Transactions.Aplication.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Arkano.Transactions.Aplication.Common;

namespace Arkano.Transactions.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [SwaggerTag("Servicios para crear y consultar las transacciones realizadas por un usuario")]
    public class TransactionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{transactionExternalId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultRequest<CheckTransactionStateDto>>> GetByExternalIdAsync(Guid transactionExternalId, [FromQuery] DateTime createdAt)
        {
            var result = await _mediator.Send(new TransactionQuery(transactionExternalId, createdAt));
            if (!result.Success)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultRequest<Guid>>> CreateAsync([FromBody] CreateTransactionCommand transactionCommand)
        {
            var result = await _mediator.Send(transactionCommand);
            if (!result.Success)
            {
                return StatusCode(result.StatusCode, result);
            }

            return StatusCode(result.StatusCode, result);
        }
    }
}
