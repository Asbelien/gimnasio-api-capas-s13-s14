using Gimnasio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class MembresiaRepositorio(GimnasioContext context) : IMembresiaRepositorio
{
    public async Task<List<Membresia>> ObtenerTodasAsync(string? tipoPlan, bool? vigente)
    {
        var query = context.Membresias.AsNoTracking().Include(m => m.Socio).AsQueryable();

        if (!string.IsNullOrWhiteSpace(tipoPlan))
            query = query.Where(m => m.TipoPlan == tipoPlan);

        if (vigente.HasValue)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            query = vigente.Value
                ? query.Where(m => m.FechaInicio <= hoy && m.FechaFin >= hoy)
                : query.Where(m => m.FechaFin < hoy || m.FechaInicio > hoy);
        }

        return await query.OrderBy(m => m.FechaInicio).ToListAsync();
    }

    public async Task<List<Membresia>> ObtenerPorSocioAsync(int socioId)
    {
        return await context.Membresias
            .AsNoTracking()
            .Include(m => m.Socio)
            .Where(m => m.SocioId == socioId)
            .OrderBy(m => m.FechaInicio)
            .ToListAsync();
    }

    public async Task<Membresia?> ObtenerPorIdAsync(int id)
    {
        return await context.Membresias
            .Include(m => m.Socio)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> ExisteSolapamientoAsync(int socioId, DateOnly fechaInicio, DateOnly fechaFin, int? excluirId = null)
    {
        return await context.Membresias.AnyAsync(m =>
            m.SocioId == socioId &&
            (excluirId == null || m.Id != excluirId) &&
            m.FechaInicio <= fechaFin &&
            m.FechaFin >= fechaInicio);
    }

    public async Task AgregarAsync(Membresia membresia)
    {
        await context.Membresias.AddAsync(membresia);
    }

    public void Eliminar(Membresia membresia)
    {
        context.Membresias.Remove(membresia);
    }

    public async Task GuardarCambiosAsync()
    {
        await context.SaveChangesAsync();
    }
}