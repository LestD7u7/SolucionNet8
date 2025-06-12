using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;

namespace BancaLafise.Infrastructure.Repository
{
    public class ReglaCumplimientoRepository : BaseRepository<ReglaCumplimiento>, IReglaCumplimientoRepository
    {
        public ReglaCumplimientoRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
