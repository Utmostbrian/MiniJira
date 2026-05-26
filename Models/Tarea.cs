using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniJira.Models;

public class Tarea
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 150 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name = "Estado")]
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;

    [Display(Name = "Prioridad")]
    public Prioridad Prioridad { get; set; } = Prioridad.Media;

    [Display(Name = "Fecha Límite")]
    [DataType(DataType.Date)]
    public DateTime? FechaLimite { get; set; }

    [Required]
    [Display(Name = "Proyecto")]
    public int ProyectoId { get; set; }

    [ForeignKey(nameof(ProyectoId))]
    public Proyecto? Proyecto { get; set; }
}

public enum EstadoTarea
{
    Pendiente,
    EnProgreso,
    Completada,
    Bloqueada
}

public enum Prioridad
{
    Baja,
    Media,
    Alta,
    Critica
}
