using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniJira.Models;

namespace MiniJira.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Tarea> Tareas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Descripcion).HasMaxLength(500);
        });

        builder.Entity<Tarea>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Titulo).IsRequired().HasMaxLength(150);
            entity.Property(t => t.Descripcion).HasMaxLength(1000);
            entity.HasOne(t => t.Proyecto)
                  .WithMany(p => p.Tareas)
                  .HasForeignKey(t => t.ProyectoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
