using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class UsuarioRepositorio(GimnasioContext context) : IUsuarioRepositorio
{
    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
    {
        return await context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await context.Usuarios.AddAsync(usuario);
    }

    public async Task GuardarCambiosAsync()
    {
        await context.SaveChangesAsync();
    }
}