using Gimnasio.Entidades;

namespace Gimnasio.Datos.Repositorios;

public interface ISocioRepositorio
{
    Task<List<Socio>> ObtenerTodosAsync(string? nombre, string? dni);

    Task<Socio?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteAsync(int id);

    Task<bool> ExisteDniAsync(string dni, int? excluirId = null);

    Task<bool> TieneMembresiasAsync(int socioId);

    Task AgregarAsync(Socio socio);

    void Eliminar(Socio socio);

    Task GuardarCambiosAsync();
}