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

namespace App.Services
{
    public interface IAbbreviationService : IBaseService<Abbreviation, string>
    {
        Task<Abbreviation> CreateAsync(CreateAbbreviationCmd cmd);
        Task<Abbreviation> GetAsync(string shortForm);
        Task<IEnumerable<Abbreviation>> GetAsync();
        Task DeleteAsync(string shortForm);
        Task<Abbreviation> ChangeAsync(string shortForm, ChangeAbbreviationCmd cmd);
    }
    public class AbbreviationService : BaseService<IAbbreviationRepository, Abbreviation, string>, IAbbreviationService
    {
        /// <summary>
        /// Creates a configuration service
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="logger">The logger</param>
        public AbbreviationService(
            IAbbreviationRepository repository,
            ILogger<IAbbreviationService> logger,
            IEventPublisher eventPublisher)
            : base(repository, eventPublisher, logger)
        {
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
    }
}
