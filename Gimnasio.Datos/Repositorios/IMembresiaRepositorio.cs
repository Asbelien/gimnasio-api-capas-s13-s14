using Gimnasio.Entidades;

namespace Gimnasio.Datos.Repositorios;

public interface IMembresiaRepositorio
{
    Task<List<Membresia>> ObtenerTodasAsync(string? tipoPlan, bool? vigente);

    Task<List<Membresia>> ObtenerPorSocioAsync(int socioId);

    Task<Membresia?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteSolapamientoAsync(int socioId, DateOnly fechaInicio, DateOnly fechaFin, int? excluirId = null);

    Task AgregarAsync(Membresia membresia);

    void Eliminar(Membresia membresia);

    Task GuardarCambiosAsync();
}