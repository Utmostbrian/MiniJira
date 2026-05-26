using Microsoft.EntityFrameworkCore;
using MiniJira.Data;
using MiniJira.Models;
using MiniJira.Repositories.Interfaces;

namespace MiniJira.Repositories.Implementations;

public class TareaRepository : ITareaRepository
{
    private readonly ApplicationDbContext _context;

    public TareaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tarea>> ObtenerTodosPorProyectoAsync(int proyectoId)
    {
        return await _context.Tareas
            .Where(t => t.ProyectoId == proyectoId)
            .OrderByDescending(t => t.Prioridad)
            .ToListAsync();
    }

    public async Task<Tarea?> ObtenerPorIdAsync(int id)
    {
        return await _context.Tareas
            .Include(t => t.Proyecto)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task CrearAsync(Tarea tarea)
    {
        await _context.Tareas.AddAsync(tarea);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Tarea tarea)
    {
        _context.Tareas.Update(tarea);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is not null)
        {
            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Tareas.AnyAsync(t => t.Id == id);
    }
}
