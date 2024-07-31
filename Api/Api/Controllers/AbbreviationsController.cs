using App.Commands;
using App.Entities;
using App.Services;
using MassTransit.SagaStateMachine;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AbbreviationsController : ControllerBase
    {
        /// <summary>
        /// The abbreviation service
        /// </summary>
        private readonly IAbbreviationService _service;

        /// <summary>
        /// Creats a new abbreviation controller
        /// </summary>
        /// <param name="service">The abbreviation service</param>
        public AbbreviationsController(IAbbreviationService service) 
        { 
            _service = service;
        }

        /// <summary>
        /// Gets all abbreviations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(statusCode: ((int)HttpStatusCode.OK), Type = typeof(Abbreviation))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAsync());
        }

        /// <summary>
        /// Gets the abbreviation from the short form
        /// </summary>
        /// <param name="shortForm">The short form i.e. cd for compact disk</param>
        /// <returns></returns>
        [HttpGet("{shortForm}")]
        public async Task<IActionResult> Get([FromRoute]string shortForm)
        {
            return Ok(await _service.GetAsync(shortForm));
        }

        /// <summary>
        /// Creates a new abbreviation
        /// </summary>
        /// <param name="cmd">The abbreviation create command</param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] CreateAbbreviationCmd cmd)
        {
            return Ok(await _service.CreateAsync(cmd));
        }

        [HttpPut("{shortForm}")]
        public async Task<IActionResult> Put([FromRoute] string shortForm, [FromBody] ChangeAbbreviationCmd cmd)
        {
            return Ok(await _service.ChangeAsync(shortForm, cmd));
        }

        [HttpDelete("{shortForm}")]
        public async Task<IActionResult> Delete([FromRoute] string shortForm)
        {
            await _service.DeleteAsync(shortForm);
            return Ok();
        }
        [HttpPut("{shortForm}/links")]
        public async Task<IActionResult> PutLinks([FromRoute] string shortForm, [FromBody] ChangeAbbreviationLinksCmd cmd)
        {
            return Ok(await _service.ChangeLinksAsync(shortForm, cmd));
        }

        [HttpGet("{shortForm}/html/{configId}")]
        public async Task<IActionResult> GetHtml([FromRoute] string shortForm, [FromRoute] Guid configId)
        {
            return Ok(await _service.GetHtmlAsync(shortForm, configId));
        }

        [HttpGet("{shortForm}/markdown/{configId}")]
        public async Task<IActionResult> GetMarkdown([FromRoute] string shortForm, [FromRoute] Guid configId)
        {
            return Ok(await _service.GetMarkdownAsync(shortForm, configId));
        }
    }
}
