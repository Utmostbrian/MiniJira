using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniJira.Models;
using MiniJira.Repositories.Interfaces;

namespace MiniJira.Controllers;

[Authorize]
public class TareasController : Controller
{
    private readonly ITareaRepository _tareaRepo;
    private readonly IProyectoRepository _proyectoRepo;

    public TareasController(ITareaRepository tareaRepo, IProyectoRepository proyectoRepo)
    {
        _tareaRepo = tareaRepo;
        _proyectoRepo = proyectoRepo;
    }

    public async Task<IActionResult> Create(int proyectoId)
    {
        var proyecto = await _proyectoRepo.ObtenerPorIdAsync(proyectoId);
        if (proyecto is null) return NotFound();

        var tarea = new Tarea { ProyectoId = proyectoId };
        ViewBag.ProyectoNombre = proyecto.Nombre;
        return View(tarea);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tarea tarea)
    {
        if (!ModelState.IsValid)
        {
            var proyecto = await _proyectoRepo.ObtenerPorIdAsync(tarea.ProyectoId);
            ViewBag.ProyectoNombre = proyecto?.Nombre;
            return View(tarea);
        }

        await _tareaRepo.CrearAsync(tarea);
        TempData["Exito"] = $"Tarea \"{tarea.Titulo}\" creada correctamente.";
        return RedirectToAction("Details", "Proyectos", new { id = tarea.ProyectoId });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tarea = await _tareaRepo.ObtenerPorIdAsync(id);
        if (tarea is null) return NotFound();
        ViewBag.ProyectoNombre = tarea.Proyecto?.Nombre;
        return View(tarea);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tarea tarea)
    {
        if (id != tarea.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            var proyecto = await _proyectoRepo.ObtenerPorIdAsync(tarea.ProyectoId);
            ViewBag.ProyectoNombre = proyecto?.Nombre;
            return View(tarea);
        }

        var existe = await _tareaRepo.ExisteAsync(id);
        if (!existe) return NotFound();

        await _tareaRepo.ActualizarAsync(tarea);
        TempData["Exito"] = "Tarea actualizada correctamente.";
        return RedirectToAction("Details", "Proyectos", new { id = tarea.ProyectoId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tarea = await _tareaRepo.ObtenerPorIdAsync(id);
        if (tarea is null) return NotFound();

        int proyectoId = tarea.ProyectoId;
        await _tareaRepo.EliminarAsync(id);
        TempData["Exito"] = "Tarea eliminada correctamente.";
        return RedirectToAction("Details", "Proyectos", new { id = proyectoId });
    }

    // API endpoint: retorna tareas de un proyecto en JSON
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Api(int proyectoId)
    {
        var tareas = await _tareaRepo.ObtenerTodosPorProyectoAsync(proyectoId);
        var resultado = tareas.Select(t => new
        {
            t.Id,
            t.Titulo,
            t.Descripcion,
            Estado = t.Estado.ToString(),
            Prioridad = t.Prioridad.ToString(),
            FechaLimite = t.FechaLimite.HasValue ? t.FechaLimite.Value.ToString("yyyy-MM-dd") : null,
            t.ProyectoId
        });
        return Json(resultado);
    }
}
