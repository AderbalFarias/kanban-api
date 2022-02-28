using Kanban.Api.Models;
using Kanban.Domain.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kanban.Api.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginController> _logger;

        public LoginController
        (
            ITokenService tokenService,
            ILogger<LoginController> logger
        )
        {
            _tokenService = tokenService;
            _logger = logger;
        }


        [HttpPost]
        [EnableCors]
        [Route("login")]
        public IActionResult Add(UserLogin model)
        {           
            return Ok(_tokenService.Generate(model.MatToEntity()));
        }
    }
}
