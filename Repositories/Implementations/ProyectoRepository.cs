using Microsoft.EntityFrameworkCore;
using MiniJira.Data;
using MiniJira.Models;
using MiniJira.Repositories.Interfaces;

namespace MiniJira.Repositories.Implementations;

public class ProyectoRepository : IProyectoRepository
{
    private readonly ApplicationDbContext _context;

    public ProyectoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proyecto>> ObtenerTodosAsync()
    {
        return await _context.Proyectos
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Proyecto>> ObtenerTodosConTareasAsync()
    {
        return await _context.Proyectos
            .Include(p => p.Tareas)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Proyecto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Proyectos.FindAsync(id);
    }

    public async Task<Proyecto?> ObtenerConTareasAsync(int id)
    {
        return await _context.Proyectos
            .Include(p => p.Tareas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CrearAsync(Proyecto proyecto)
    {
        await _context.Proyectos.AddAsync(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Proyecto proyecto)
    {
        _context.Proyectos.Update(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto is not null)
        {
            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Proyectos.AnyAsync(p => p.Id == id);
    }
}
