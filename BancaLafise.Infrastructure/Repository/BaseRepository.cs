using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Common;
using BancaLafise.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BancaLafise.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly LafiseDbContext _context;

        public BaseRepository(LafiseDbContext context)
        {
            _context = context;
        }

        public async Task Create(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateRange(List<T> entities, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(T entity, CancellationToken cancellationToken)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<T> Get(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }
    }
}
