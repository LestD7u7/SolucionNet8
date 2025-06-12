using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;

namespace BancaLafise.Infrastructure.Repository
{
    public class TipoCuentaRepository : BaseRepository<TipoCuenta>, ITipoCuentaRepository
    {
        public TipoCuentaRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
