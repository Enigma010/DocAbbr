using App.Entities;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AbbreviationController : ControllerBase
    {
        private readonly IAbbreviationService _service;
        public AbbreviationController(IAbbreviationService service) 
        { 
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: ((int)HttpStatusCode.OK), Type = typeof(Abbreviation))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{shortForm}")]
        public async Task<IActionResult> Get([FromRoute]string shortForm)
        {
            return Ok(await _service.GetAsync(shortForm));
        }
    }
}
