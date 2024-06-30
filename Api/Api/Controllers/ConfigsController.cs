using App.Commands;
using App.Entities;
using App.Services;
using Db;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigsController : ControllerBase
    {
        /// <summary>
        /// The service for working with configurations
        /// </summary>
        private readonly IConfigService _service;
        /// <summary>
        /// Creates a new configuraiton controller
        /// </summary>
        /// <param name="service">The configuraiton service</param>
        public ConfigsController(IConfigService service) 
        { 
            _service = service;
        }
        /// <summary>
        /// HTTP POST request to create a configuration
        /// </summary>
        /// <param name="cmd">The create command</param>
        /// <returns>The configuration</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]CreateConfigCmd cmd)
        {
            Config config = await _service.CreateAsync(cmd);
            return Ok(config);
        }
        /// <summary>
        /// HTTP GET to getsa specific configuration by ID
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns>The configuration</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _service.GetAsync(id));
            }
            catch (DbEntityNotFoundException<Guid>)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// HTTP GET to return all the configurations
        /// </summary>
        /// <returns>The configurations</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _service.GetAsync());
        }
        /// <summary>
        /// HTTP PUT to update a specific configuration
        /// </summary>
        /// <param name="id">The configuration ID</param>
        /// <param name="cmd">The change command</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, 
            [FromBody] ChangeConfigCmd cmd)
        {
            Config config = await _service.ChangeAsync(id, cmd);
            return Ok(config);
        }
        /// <summary>
        /// HTTP DELETE to delete a specific configuration by ID
        /// </summary>
        /// <param name="id">The ID of the configuration</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
