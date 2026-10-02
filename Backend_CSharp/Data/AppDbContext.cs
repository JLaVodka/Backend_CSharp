using Backend_CSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_CSharp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Tarea> Tareas => Set<Tarea>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("empleados_empleado");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Especialidad)
                .HasColumnName("especialidad")
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.ToTable("tareas_tarea");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .HasColumnName("id");

            entity.Property(t => t.Titulo)
                .HasColumnName("titulo")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(t => t.Descripcion)
                .HasColumnName("descripcion")
                .IsRequired();

            entity.Property(t => t.Estado)
                .HasColumnName("estado")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(t => t.EmpleadoId)
                .HasColumnName("empleado_id");
        });
    }
}