using App.Commands;
using App.Entities;
using App.Repositories;
using AppCore.Services;
using EventBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork;
using Logging;
using Markdown;

namespace App.Services
{
    public interface IAbbreviationService : IBaseService<Abbreviation, string>
    {
        Task<Abbreviation> CreateAsync(CreateAbbreviationCmd cmd);
        Task<Abbreviation> GetAsync(string shortForm);
        Task<IEnumerable<Abbreviation>> GetAsync();
        Task DeleteAsync(string shortForm);
        Task<Abbreviation> ChangeAsync(string shortForm, ChangeAbbreviationCmd cmd);
        Task<List<string>> GetMarkdownAsync(string shortForm, Guid configurationId);
        Task<List<string>> GetHtmlAsync(string shortForm, Guid configurationId);

    }
    public class AbbreviationService : BaseService<IAbbreviationRepository, Abbreviation, string>, IAbbreviationService
    {
        private readonly IConfigService _configService;
        private readonly IMarkdownClient _markdownClient;
        /// <summary>
        /// Creates a configuration service
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="logger">The logger</param>
        public AbbreviationService(
            IAbbreviationRepository repository,
            ILogger<IAbbreviationService> logger,
            IConfigService configService,
            IMarkdownClient markdownClient,
            IEventPublisher eventPublisher)
            : base(repository, eventPublisher, logger)
        {
            _configService = configService;
            _markdownClient = markdownClient;
        }

        /// <summary>
        /// Creates a new configuration with all of the defaults
        /// </summary>
        /// <param name="cmd">The create config command</param>
        /// <returns>The new configuration object</returns>
        public async Task<Abbreviation> CreateAsync(CreateAbbreviationCmd cmd)
        {
            using (_logger.LogCaller())
            {
                using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
                {
                    return await unitOfWorks.RunAsync(async () =>
                    {
                        Abbreviation abbreviation = new Abbreviation(cmd.ShortForm);
                        await _repository.InsertAsync(abbreviation);
                        await PublishEvents(abbreviation);
                        return abbreviation;
                    });
                }
            }
        }

        /// <summary>
        /// Deletes a configuration
        /// </summary>
        /// <param name="shortForm">The short form of the abbreviation</param>
        /// <returns></returns>
        public async Task DeleteAsync(string shortForm)
        {
            using (_logger.LogCaller())
            {
                using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
                {
                    await unitOfWorks.RunAsync(async () =>
                    {
                        Abbreviation abbreviation = await _repository.GetAsync(shortForm);
                        await _repository.DeleteAsync(abbreviation);
                        await PublishEvents(abbreviation);
                        await unitOfWorks.Commit();
                    });
                }
            }
        }

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="shortForm">The short form of the abbreviation</param>
        /// <returns></returns>
        public async Task<Abbreviation> GetAsync(string shortForm)
        {
            using (_logger.LogCaller())
            {
                return await _repository.GetAsync(shortForm);
            }
        }

        /// <summary>
        /// Gets all of the configurations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Abbreviation>> GetAsync()
        {
            using (_logger.LogCaller())
            {
                return await _repository.GetAsync();
            }
        }

        /// <summary>
        /// Changes or updates a configuration
        /// </summary>
        /// <param name="shortForm">The short form of the abbreviation</param>
        /// <param name="change">The change that is occurring</param>
        /// <returns>The updated configuration</returns>
        public async Task<Abbreviation> ChangeAsync(string shortForm, ChangeAbbreviationCmd change)
        {
            return await ChangeAsync(shortForm, (abbreviation) =>
            {
                abbreviation.Change(change);
                return abbreviation;
            });
        }
        /// <summary>
        /// Converts the markdown to HTML
        /// </summary>
        /// <param name="shortForm"></param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetMarkdownAsync(string shortForm, Guid configurationId)
        {
            Abbreviation abbreviation = await GetAsync(shortForm);
            Config config = await _configService.GetAsync(configurationId);
            return abbreviation.Markdown(config).ToList();
        }
        /// <summary>
        /// Converts the markdown to HTML
        /// </summary>
        /// <param name="shortForm"></param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetHtmlAsync(string shortForm, Guid configurationId)
        {
            List<string> markdown = await GetMarkdownAsync(shortForm, configurationId);
            string html = _markdownClient.ToHtml(string.Join("\n", markdown));
            return html.Split("\n").ToList(); 
        }
    }
}
