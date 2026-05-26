using MiniJira.Models;

namespace MiniJira.Repositories.Interfaces;

public interface ITareaRepository
{
    Task<IEnumerable<Tarea>> ObtenerTodosPorProyectoAsync(int proyectoId);
    Task<Tarea?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Tarea tarea);
    Task ActualizarAsync(Tarea tarea);
    Task EliminarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
