using Kanban.Data.Contexts;
using Kanban.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kanban.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DbContext _context;

        public BaseRepository(KanbanContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class => _context.Set<TEntity>();

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>() where TEntity : class =>
            await _context.Set<TEntity>().ToListAsync();

        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class =>
            _context.Set<TEntity>().Where(predicate);

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class =>
            await _context.Set<TEntity>().Where(predicate).ToListAsync();

        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, string include) where TEntity : class =>
            _context.Set<TEntity>().Include(include).Where(predicate);

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, string include) where TEntity : class =>
            await _context.Set<TEntity>().Where(predicate).Include(include).ToListAsync();

        public TEntity GetObject<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class =>
            _context.Set<TEntity>().FirstOrDefault(predicate);

        public async Task<TEntity> GetObjectAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class =>
            await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public bool GetAny<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class =>
            _context.Set<TEntity>().Any(predicate);

        public async Task<bool> GetAnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class =>
            await _context.Set<TEntity>().AnyAsync(predicate);

        public async Task<TEntity> Add<TEntity>(TEntity entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<int> Update<TEntity>(TEntity entity)
        {
            _context.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete<TEntity>(TEntity entity)
        {
            _context.Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}