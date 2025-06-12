using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;

namespace BancaLafise.Infrastructure.Repository
{
    public class TipoMovimientoRepository : BaseRepository<TipoMovimiento>, ITipoMovimientoRepository
    {
        public TipoMovimientoRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
