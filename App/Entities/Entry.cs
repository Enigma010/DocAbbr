﻿using App.Commands;
using App.Mappers;
using App.Repositories.Dtos;
using AppCore;
using AppEvents;
using Markdig.Renderers.Html;
using MassTransit.SagaStateMachine;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities
{
    /// <summary>
    /// An entry
    /// </summary>
    public class Entry : Entity<EntryDto, string>
    {
        /// <summary>
        /// Creates a default entry
        /// </summary>
        public Entry() : base(() => { return string.Empty;  })
        {
        }

        /// <summary>
        /// Creates an entry
        /// </summary>
        /// <param name="dto">The data transfer object</param>
        public Entry(EntryDto dto): base(dto) 
        { 
        }

        /// <summary>
        /// Creates a new entry
        /// </summary>
        /// <param name="name">The short form, i.e. cd</param>
        /// <param name="description">The description</param>
        public Entry(string name, 
            IEnumerable<string> description) : base(() => { return name; })
        {
            _dto.Description = description.ToList();
            AddEvent(new EntryCreatedEvent(Id, name, description));
        }

        /// <summary>
        /// Gets the name of the entry, for example CD
        /// for compact disk
        /// </summary>
        public string Name => _dto.Id;
        
        /// <summary>
        /// The description of the entry
        /// </summary>
        public IReadOnlyCollection<string> Description => _dto.Description.AsReadOnly();
        
        /// <summary>
        /// Changes an entry
        /// </summary>
        /// <param name="cmd">The change entry command</param>
        public void Change(ChangeEntryCmd cmd)
        {
            if (Description != cmd.Description)
            {
                AddEvent(new EntryChangedEvent(Id, Name, Description, cmd.Description));
                _dto.Description = cmd.Description;
            }
        }
        
        /// <summary>
        /// Gets the markdown
        /// </summary>
        /// <param name="config">The configuraiton</param>
        /// <returns>The markdown</returns>
        public IEnumerable<string> Markdown(Config config)
        {
            StringBuilder markdown = new StringBuilder(string.Join('\n', config.Markdown));
            markdown = markdown.Replace(Config.NameTemplate, Name);
            markdown = markdown.Replace(Config.DescriptionTemplate, string.Join('\n', Description));
            StringBuilder referenceLinksMarkdown = new StringBuilder();
            return markdown.ToString().Split('\n');
        }

        /// <summary>
        /// Delete an entry
        /// </summary>
        public override void Deleted()
        {
            AddEvent(new EntryDeletedEvent(
                _dto.Id,
                _dto.Name,
                _dto.Description));
        }
    }
}
