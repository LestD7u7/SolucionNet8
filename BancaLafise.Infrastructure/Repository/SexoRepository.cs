using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;

namespace BancaLafise.Infrastructure.Repository
{
    public class SexoRepository : BaseRepository<Sexo>, ISexoRepository
    {
        public SexoRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
