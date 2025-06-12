using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;

namespace BancaLafise.Infrastructure.Repository
{
    public class TipoTransaccionRepository : BaseRepository<TipoTransaccion>, ITipoTransaccionRepository
    {
        public TipoTransaccionRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
