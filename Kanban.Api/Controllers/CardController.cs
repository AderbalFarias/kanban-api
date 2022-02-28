using Kanban.Api.Models;
using Kanban.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kanban.Api.Controllers
{
    /// <summary>
    /// Controller to allow kanban cards management 
    /// </summary>
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //[Route("api/v{version:apiVersion}")]
    //[ApiVersion("1.0")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ILogger<CardController> _logger;

        public CardController
        (
            ICardService cardService,
            ILogger<CardController> logger
        )
        {
            _cardService = cardService;
            _logger = logger;
        }

        /// <summary>
        /// Get all card records from database
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     GET [Api base url]/cards/
        /// 
        /// Sample Response:
        /// 
        ///     [
        ///         {
        ///             Id: "c69a92d8-476e-4d29-bb6c-845cf703b032",
        ///             Titulo: "x1",
        ///             Conteudo: "x2",
        ///             Lista: "x3"
        ///         },
        ///         {
        ///             Id: "a6c5d95b-e8ed-47b3-b66f-2bf7962286db",
        ///             Titulo: "y1",
        ///             Conteudo: "y2",
        ///             Lista: "y3"
        ///         }
        ///     ]
        ///     
        /// </remarks>
        /// <response code="200">Returns a collection of cards</response>
        /// <response code="401">Access Unauthorized</response>
        /// <response code="500">Internal error</response>
        /// <returns>Returns cards</returns>
        /// <param></param>
        [HttpGet]
        [EnableCors]
        [Route("cards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync() => Ok(await _cardService.GetAllAsync());

        /// <summary>
        /// Get a specific card based on its id
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     GET [Api base url]/cards/{id}
        /// 
        /// Sample Response:
        /// 
        ///     [
        ///         {
        ///             Id: "c69a92d8-476e-4d29-bb6c-845cf703b032",
        ///             Titulo: "x1",
        ///             Conteudo: "x2",
        ///             Lista: "x3"
        ///         }
        ///     ]
        ///     
        /// </remarks>
        /// <response code="200">Returns a collection of cards</response>
        /// <response code="401">Access Unauthorized</response>
        /// <response code="500">Internal error</response>
        /// <returns>Returns the card</returns>
        /// <param name="id">Card Identifier</param>
        [HttpGet]
        [EnableCors]
        [Route("cards/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _cardService.GetByIdAsync(id));

        /// <summary>
        /// Create a new card on kanban
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     POST [Api base url]/cards
        ///     
        /// Sample Model:
        /// 
        ///     {
        ///         Titulo: "x1",
        ///         Conteudo: "x2",
        ///         Lista: "x3"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns a collection of cards</response>
        /// <response code="400">Validation failed</response>
        /// <response code="401">Access Unauthorized</response>
        /// <response code="500">Internal error</response>
        /// <returns>Returns cards</returns>
        /// <param name="model">Card model</param>
        [HttpPost]
        [EnableCors]
        [Route("cards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(Card model)
        {
            if (!ModelState.IsValid || model.Id != Guid.Empty)
            {
                _logger.LogWarning($"Post request inválido {typeof(CardController).Name}");

                return BadRequest();
            }

            var card = await _cardService.AddAsync(model.MapToEntityAdd());

            return Ok(card);
        }

        /// <summary>
        /// Update the card
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     PUT [Api base url]/cards/{id}
        ///     
        /// Sample Model:
        /// 
        ///     {
        ///         Id: "c69a92d8-476e-4d29-bb6c-845cf703b032",
        ///         Titulo: "x1",
        ///         Conteudo: "x2",
        ///         Lista: "x3"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns a collection of cards</response>
        /// <response code="400">Validation failed</response>
        /// <response code="401">Access Unauthorized</response>
        /// <response code="404">Card not found</response>
        /// <response code="500">Internal error</response>
        /// <returns>Returns card</returns>
        /// <param name="id">Card Identifier</param>
        /// <param name="model">Card Model</param>
        [HttpPut]
        [EnableCors]
        [Route("cards/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, Card model)
        {
            if (model.Id == Guid.Empty)
                return NotFound();

            if (!ModelState.IsValid || model.Id != id)
            {
                _logger.LogWarning($"Put request inválido {typeof(CardController).Name}");

                return BadRequest();
            }

            try
            {
                await _cardService.UpdateAsync(model.MapToEntity());
                _logger.LogInformation($"{DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")} - Card {id} - {model.Titulo} - Alterar");

                return Ok(await _cardService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return NotFound();
            }
        }

        /// <summary>
        /// Remove card from kanban
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     DELETE [Api base url]/cards/{id}
        ///     
        /// </remarks>
        /// <response code="200">Returns a collection of cards</response>
        /// <response code="400">Validation failed</response>
        /// <response code="401">Access Unauthorized</response>
        /// <response code="404">Card not found</response>
        /// <response code="500">Internal error</response>
        /// <returns>Returns card</returns>
        /// <param name="id">Card Identifier</param>
        [HttpDelete]
        [EnableCors]
        [Route("cards/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning($"Delete request inválido on {typeof(CardController).Name}");

                return NotFound();
            }

            var card = await _cardService.GetByIdAsync(id);

            if (card == null)
                return NotFound();

            await _cardService.DeleteAsync(card);
            _logger.LogInformation($"{DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")} - Card {id} - {card.Titulo} - Removido");

            return Ok(await _cardService.GetAllAsync());
        }
    }
}