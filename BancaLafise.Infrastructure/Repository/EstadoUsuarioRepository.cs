using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;

namespace BancaLafise.Infrastructure.Repository
{
    public class EstadoUsuarioRepository : BaseRepository<EstadoUsuario>, IEstadoUsuarioRepository
    {
        public EstadoUsuarioRepository(LafiseDbContext context) : base(context)
        {
        }
    }
}
