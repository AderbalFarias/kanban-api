using Kanban.Api.Models;
using Kanban.Domain.Entities;
using Kanban.Domain.Interfaces.Services;
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
    //[Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //[Route("api/v{version:apiVersion}")]
    //[ApiVersion("1.0")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ILogger _logger;

        public CardController
        (
            ICardService CardService,
            ILogger<CardController> logger
        )
        {
            _cardService = CardService;
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
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _cardService.GetAll();

            return Ok(result);
        }

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
        public async Task<IActionResult> GetById(Guid id) => Ok(await _cardService.GetById(id));

        /// <summary>
        /// Create a new card object
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     POST [Api base url]/cards
        ///     
        /// Sample Model:
        /// 
        ///     [
        ///         {
        ///             Titulo: "x1",
        ///             Conteudo: "x2",
        ///             Lista: "x3"
        ///         }
        ///     ]
        ///     
        /// </remarks>
        /// <response code="200">Returns a collection of cards</response>
        /// <response code="204">Model was null</response>
        /// <response code="400">Validation failed</response>
        /// <response code="401">Access Unauthorized</response>
        /// <response code="500">Internal error</response>
        /// <returns>Returns cards</returns>
        /// <param name="model">Card model</param>
        [HttpPost]
        [EnableCors]
        [Route("cards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(CardModel model)
        {
            var controller = typeof(CardController).Name;

            _logger.LogInformation($"Save method on {controller} started");

            if (model == null)
            {
                _logger.LogWarning($"Save method on {controller} did not find any value in the request");

                return NoContent();
            }

            await _cardService.Add(new Card
            {
                Titulo = model.Titulo,
                Conteudo = model.Conteudo,
                Lista = model.Lista
            });

            _logger.LogInformation($"Save method on {controller} finished successfully");

            return Ok();
        }

        /// <summary>
        /// Update card
        /// </summary>
        /// <remarks>
        /// Sample Request: 
        /// 
        ///     PUT [Api base url]/cards/{id}
        ///     
        /// Sample Model:
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
        public async Task<IActionResult> Update(Guid id, CardModel model)
        {
            var controller = typeof(CardController).Name;

            _logger.LogInformation($"Update method on {controller} started");

            if (model == null)
            {
                _logger.LogWarning($"Update method on {controller} did not find any value in the request");

                return NoContent();
            }

            await _cardService.Update(new Card
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Conteudo = model.Conteudo,
                Lista = model.Lista
            });

            _logger.LogInformation($"Update method on {controller} finished successfully");

            return Ok();
        }

        /// <summary>
        /// Remove Card object
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
            var controller = typeof(CardController).Name;

            _logger.LogInformation($"Delete method on {controller} started");

            await _cardService.Delete(id);

            _logger.LogInformation($"Delete method on {controller} finished successfully");

            return Ok();
        }
    }
}