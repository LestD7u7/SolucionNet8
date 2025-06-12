namespace BancaLafise.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string ClienteId { get; }
        string UsuarioId { get; }
        string Username { get; }
    }
}
