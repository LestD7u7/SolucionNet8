using BancaLafise.Application.Interfaces;
using BancaLafise.Domain.Entities;
using BancaLafise.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BancaLafise.Infrastructure.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(LafiseDbContext context) : base(context) 
        {
        }

        public async Task<bool> Exist(string NombreUsuario)
        {
            return await _context.Usuarios.AnyAsync(u => u.Nombre == NombreUsuario);
        }

        public async Task<Usuario> GetUserName(string NombreUsuario)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Nombre == NombreUsuario);
        }

        public async Task<string> HashClave(string clave)
        {
            return BCrypt.Net.BCrypt.HashPassword(clave);
        }

        public async Task<bool> ValidarClave(Usuario user,string clave)
        {
            if(string.IsNullOrEmpty(user.Clave)) return false;

            return BCrypt.Net.BCrypt.Verify(clave, user.Clave);
        }
    }
}
