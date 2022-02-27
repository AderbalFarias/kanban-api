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

        public async Task<IEnumerable<Card>> GetAll() => await _baseRepository.GetAsync<Card>();

        public async Task<Card> GetById(Guid id) =>
            await _baseRepository.GetObjectAsync<Card>(card => card.Id == id);

        public async Task Add(Card entity) => await _baseRepository.Add(entity);

        public async Task Update(Card entity)
        {
            var card = await _baseRepository.GetAnyAsync<Card>(card => card.Id == entity.Id);

            if (card)
            {
                await _baseRepository.Update(entity);
            }
            else
            {
                _logger.LogError($"Card not found Card Id: {entity.Id}");
                throw new Exception("Card not found");
            }

            //card.Titulo = entity.Titulo;
            //card.Conteudo = entity.Conteudo;
            //card.Lista = entity.Lista;
        }

        public async Task Delete(Card entity)
        {
            var card = await _baseRepository.GetAnyAsync<Card>(card => card.Id == entity.Id);

            if (card)
            {
                await _baseRepository.Delete(entity);
            }
            else
            {
                _logger.LogError($"Card not found Card Id: {entity.Id}");
                throw new Exception("Card not found");
            }
        }
    }
}

