using Kanban.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kanban.Domain.Interfaces.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAll();
        Task<Card> GetById(Guid id);
        Task Add(Card entity);
        Task Update(Card entity);
        Task Delete(Guid id);
    }
}
