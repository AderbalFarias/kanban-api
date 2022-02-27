using Kanban.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanban.Domain.Interfaces.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAllAsync();
        Task<Card> GetByIdAsync(Guid id);
        Task<Card> AddAsync(Card entity);
        Task UpdateAsync(Card entity);
        Task DeleteAsync(Card entity);
    }
}
