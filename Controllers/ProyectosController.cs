using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniJira.Models;
using MiniJira.Repositories.Interfaces;

namespace MiniJira.Controllers;

public class ProyectosController : Controller
{
    private readonly IProyectoRepository _proyectoRepo;

    public ProyectosController(IProyectoRepository proyectoRepo)
    {
        _proyectoRepo = proyectoRepo;
    }

    public async Task<IActionResult> Index()
    {
        var proyectos = await _proyectoRepo.ObtenerTodosAsync();
        return View(proyectos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var proyecto = await _proyectoRepo.ObtenerConTareasAsync(id);
        if (proyecto is null) return NotFound();
        return View(proyecto);
    }

    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Proyecto proyecto)
    {
        if (!ModelState.IsValid) return View(proyecto);

        proyecto.FechaCreacion = DateTime.Now;
        await _proyectoRepo.CrearAsync(proyecto);
        TempData["Exito"] = $"El proyecto \"{proyecto.Nombre}\" fue creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var proyecto = await _proyectoRepo.ObtenerPorIdAsync(id);
        if (proyecto is null) return NotFound();
        return View(proyecto);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Proyecto proyecto)
    {
        if (id != proyecto.Id) return BadRequest();
        if (!ModelState.IsValid) return View(proyecto);

        var existe = await _proyectoRepo.ExisteAsync(id);
        if (!existe) return NotFound();

        await _proyectoRepo.ActualizarAsync(proyecto);
        TempData["Exito"] = "Proyecto actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var proyecto = await _proyectoRepo.ObtenerConTareasAsync(id);
        if (proyecto is null) return NotFound();
        return View(proyecto);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _proyectoRepo.EliminarAsync(id);
        TempData["Exito"] = "Proyecto eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // API endpoint: retorna todos los proyectos en JSON
    [HttpGet]
    public async Task<IActionResult> Api()
    {
        var proyectos = await _proyectoRepo.ObtenerTodosConTareasAsync();
        var resultado = proyectos.Select(p => new
        {
            p.Id,
            p.Nombre,
            p.Descripcion,
            FechaCreacion = p.FechaCreacion.ToString("yyyy-MM-dd"),
            Estado = p.Estado.ToString(),
            TotalTareas = p.Tareas.Count
        });
        return Json(resultado);
    }
}
