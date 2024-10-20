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
    public class EntriesController : ControllerBase
    {
        /// <summary>
        /// The abbreviation service
        /// </summary>
        private readonly IEntryService _service;

        /// <summary>
        /// Creats a new abbreviation controller
        /// </summary>
        /// <param name="service">The abbreviation service</param>
        public EntriesController(IEntryService service) 
        { 
            _service = service;
        }

        /// <summary>
        /// Gets all abbreviations
        /// </summary>
        /// <returns>The action result</returns>
        [HttpGet]
        [ProducesResponseType(statusCode: ((int)HttpStatusCode.OK), Type = typeof(Entry))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAsync());
        }

        /// <summary>
        /// Gets the abbreviation from the short form
        /// </summary>
        /// <param name="name">The short form i.e. cd for compact disk</param>
        /// <returns>The action result</returns>
        [HttpGet("{shortForm}")]
        public async Task<IActionResult> Get([FromRoute]string name)
        {
            return Ok(await _service.GetAsync(name));
        }

        /// <summary>
        /// Creates a new abbreviation
        /// </summary>
        /// <param name="cmd">The abbreviation create command</param>
        /// <returns>The action result</returns>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] CreateEntryCmd cmd)
        {
            return Ok(await _service.CreateAsync(cmd));
        }

        /// <summary>
        /// Changes an entry
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="cmd">The change entry command</param>
        /// <returns>Ok</returns>
        [HttpPut("{name}")]
        public async Task<IActionResult> Put([FromRoute] string name, [FromBody] ChangeEntryCmd cmd)
        {
            return Ok(await _service.ChangeAsync(name, cmd));
        }

        /// <summary>
        /// Deletes and entry
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <returns>The action result</returns>
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            await _service.DeleteAsync(name);
            return Ok();
        }

        /// <summary>
        /// Gets the HTMl for the entry
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="configId">The configuration ID to use when generating the HTML</param>
        /// <returns>The action result</returns>
        [HttpGet("{name}/html/{configId}")]
        public async Task<IActionResult> GetHtml([FromRoute] string name, [FromRoute] Guid configId)
        {
            return Ok(await _service.GetHtmlAsync(name, configId));
        }

        /// <summary>
        /// Gets the markdown for the entry
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="configId">The configuration ID to use when generating the markdown</param>
        /// <returns>The action result</returns>
        [HttpGet("{name}/markdown/{configId}")]
        public async Task<IActionResult> GetMarkdown([FromRoute] string name, [FromRoute] Guid configId)
        {
            return Ok(await _service.GetMarkdownAsync(name, configId));
        }
    }
}
