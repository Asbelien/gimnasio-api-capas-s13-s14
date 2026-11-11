using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class SocioRepositorio(GimnasioContext context) : ISocioRepositorio
{
    public async Task<List<Socio>> ObtenerTodosAsync(string? nombre, string? dni)
    {
        var query = context.Socios.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(s => s.Nombre.Contains(nombre));

        if (!string.IsNullOrWhiteSpace(dni))
            query = query.Where(s => s.Dni == dni);

        return await query.OrderBy(s => s.Nombre).ToListAsync();
    }

    public async Task<Socio?> ObtenerPorIdAsync(int id)
    {
        return await context.Socios.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await context.Socios.AnyAsync(s => s.Id == id);
    }

    public async Task<bool> ExisteDniAsync(string dni, int? excluirId = null)
    {
        return await context.Socios.AnyAsync(s => s.Dni == dni && (excluirId == null || s.Id != excluirId));
    }

    public async Task<bool> TieneMembresiasAsync(int socioId)
    {
        return await context.Membresias.AnyAsync(m => m.SocioId == socioId);
    }

    public async Task AgregarAsync(Socio socio)
    {
        await context.Socios.AddAsync(socio);
    }

    public void Eliminar(Socio socio)
    {
        context.Socios.Remove(socio);
    }

    public async Task GuardarCambiosAsync()
    {
        await context.SaveChangesAsync();
    }
}