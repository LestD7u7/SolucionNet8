using BancaLafise.Domain.Entities;

namespace BancaLafise.Application.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<bool> Exist(string NombreUsuario);
        Task<Usuario> GetUserName(string NombreUsuario);
        Task<bool> ValidarClave(Usuario user, string clave);
        Task<string> HashClave(string clave);
    }
}
