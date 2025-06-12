using BancaLafise.Domain.Common;

namespace BancaLafise.Application.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task Create(T entity, CancellationToken cancellationToken);
        Task CreateRange(List<T> entities, CancellationToken cancellationToken);
        Task Update(T entity, CancellationToken cancellationToken);
        Task Delete(T entity, CancellationToken cancellationToken);
        Task<T> Get(int id, CancellationToken cancellationToken);
        Task<List<T>> GetAll(CancellationToken cancellationToken);
    }
}
