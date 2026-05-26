using MiniJira.Models;

namespace MiniJira.Repositories.Interfaces;

public interface IProyectoRepository
{
    Task<IEnumerable<Proyecto>> ObtenerTodosAsync();
    Task<IEnumerable<Proyecto>> ObtenerTodosConTareasAsync();
    Task<Proyecto?> ObtenerPorIdAsync(int id);
    Task<Proyecto?> ObtenerConTareasAsync(int id);
    Task CrearAsync(Proyecto proyecto);
    Task ActualizarAsync(Proyecto proyecto);
    Task EliminarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
