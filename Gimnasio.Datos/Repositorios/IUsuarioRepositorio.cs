using Gimnasio.Entidades;

namespace Gimnasio.Datos.Repositorios;

public interface IUsuarioRepositorio
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAsync(Usuario usuario);
    Task GuardarCambiosAsync();
}