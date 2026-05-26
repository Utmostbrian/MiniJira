using System.ComponentModel.DataAnnotations;

namespace MiniJira.Models;

public class Proyecto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    [Display(Name = "Nombre del Proyecto")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Fecha de Creación")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Display(Name = "Estado")]
    public EstadoProyecto Estado { get; set; } = EstadoProyecto.Activo;

    public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}

public enum EstadoProyecto
{
    Activo,
    Pausado,
    Completado
}
