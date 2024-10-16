using App.Commands;
using App.Entities;
using App.Repositories;
using App.Repositories.Dtos;
using AppCore.Services;
using EventBus;
using Logging;
using Markdown;
using Microsoft.Extensions.Logging;
using UnitOfWork;

namespace App.Services
{
    public interface IEntryService : IBaseService<Entry, string>
    {
        Task<Entry> CreateAsync(CreateEntryCmd cmd);
        Task<Entry> GetAsync(string name);
        Task<IEnumerable<Entry>> GetAsync();
        Task DeleteAsync(string shortForm);
        Task<Entry> ChangeAsync(string shortForm, ChangeEntryCmd cmd);
        Task<Entry> ChangeLinksAsync(string shortForm, ChangeEntryLinksCmd cmd);
        Task<List<string>> GetMarkdownAsync(string shortForm, Guid configurationId);
        Task<List<string>> GetHtmlAsync(string shortForm, Guid configurationId);

    }
    public class EntryService : BaseService<IEntryRepository, Entry, EntryDto, string>, IEntryService
    {
        private readonly IConfigService _configService;
        private readonly IMarkdownClient _markdownClient;
        /// <summary>
        /// Creates a configuration service
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="logger">The logger</param>
        public EntryService(
            IEntryRepository repository,
            ILogger<IEntryService> logger,
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
        public async Task<Entry> CreateAsync(CreateEntryCmd cmd)
        {
            using (_logger.LogCaller())
            {
                using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
                {
                    return await unitOfWorks.RunAsync(async () =>
                    {
                        Entry entry = new Entry(cmd.Name, cmd.Description);
                        await _repository.InsertAsync(entry);
                        await PublishEvents(entry);
                        return entry;
                    });
                }
            }
        }

        /// <summary>
        /// Deletes a entry
        /// </summary>
        /// <param name="name">The name of the entry/param>
        /// <returns></returns>
        public async Task DeleteAsync(string name)
        {
            using (_logger.LogCaller())
            {
                using (var unitOfWorks = new UnitOfWorks(_unitOfWorks, _logger))
                {
                    await unitOfWorks.RunAsync(async () =>
                    {
                        Entry entry = await _repository.GetAsync(name);
                        await _repository.DeleteAsync(entry);
                        await PublishEvents(entry);
                        await unitOfWorks.Commit();
                    });
                }
            }
        }

        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <returns></returns>
        public async Task<Entry> GetAsync(string name)
        {
            using (_logger.LogCaller())
            {
                return await _repository.GetAsync(name);
            }
        }

        /// <summary>
        /// Gets all of the configurations
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Entry>> GetAsync()
        {
            using (_logger.LogCaller())
            {
                return await _repository.GetAsync();
            }
        }

        /// <summary>
        /// Changes or updates an entry
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="cmd">The change that is occurring</param>
        /// <returns>The updated entry</returns>
        public async Task<Entry> ChangeAsync(string name, ChangeEntryCmd cmd)
        {
            return await ChangeAsync(name, (entry) =>
            {
                entry.Change(cmd);
                return entry;
            });
        }
        /// <summary>
        /// Changes or updates an entry links
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="cmd">The change that is occurring</param>
        /// <returns>The updated entry</returns>
        public async Task<Entry> ChangeLinksAsync(string name, ChangeEntryLinksCmd cmd)
        {
            return await ChangeAsync(name, (entry) =>
            {
                entry.ChangeLinks(cmd);
                return entry;
            });
        }
        /// <summary>
        /// Converts the markdown to HTML
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetMarkdownAsync(string name, Guid configurationId)
        {
            Entry entry = await GetAsync(name);
            Config config = await _configService.GetAsync(configurationId);
            return entry.Markdown(config).ToList();
        }
        /// <summary>
        /// Converts the markdown to HTML
        /// </summary>
        /// <param name="name">The name of the entry</param>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetHtmlAsync(string name, Guid configurationId)
        {
            List<string> markdown = await GetMarkdownAsync(name, configurationId);
            string html = _markdownClient.ToHtml(string.Join("\n", markdown));
            return html.Split("\n").ToList(); 
        }
    }
}
