using Kanban.Domain.Entities;
using Kanban.Domain.Interfaces.Repositories;
using Kanban.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanban.Domain.Services
{
    public class CardService : ICardService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly ILogger _logger;

        public CardService(IBaseRepository baseRepository, ILogger<CardService> logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Card>> GetAllAsync() => await _baseRepository.GetAsync<Card>();

        public async Task<Card> GetByIdAsync(Guid id) =>
            await _baseRepository.GetObjectAsync<Card>(card => card.Id == id);

        public async Task<Card> AddAsync(Card entity) => await _baseRepository.Add(entity);

        public async Task UpdateAsync(Card entity)
        {
            var card = await _baseRepository.GetAnyAsync<Card>(card => card.Id == entity.Id);

            if (card)
            {
                await _baseRepository.Update(entity);
            }
            else
            {
                _logger.LogError($"Card Id not found in the database: {entity.Id}");
                throw new Exception("Card not found in the database");
            }
        }

        public async Task DeleteAsync(Card entity)
        {
            if (entity != null)
            {
                await _baseRepository.Delete(entity);
            }
            else
            {
                _logger.LogError($"Card Id not found in the database: {entity.Id}");
                throw new Exception("Card not found in the database");
            }
        }
    }
}

