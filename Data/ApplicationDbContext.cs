using ExamenSATT.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ExamenSATT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        // Tablas
        public DbSet<EmpleadoModel> Empleados { get; set; }
        public DbSet<AreaModel> Areas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AreaModel>().ToTable("TA_AREA").HasKey(a => a.id_area);
            modelBuilder.Entity<EmpleadoModel>().ToTable("TA_EMPL").HasKey(e => e.id_empl);
            modelBuilder.Entity<EmpleadoModel>()
                .HasOne<AreaModel>() // Un empleado tiene un área
                .WithMany()          // Un área puede tener muchos empleados
                .HasForeignKey(e => e.id_area); // La columna real en la DB es id_area

            modelBuilder.Entity<EmpleadoModel>(entity =>
            {
                // Esto le dice a EF: "Cuando llenes este modelo, busca una columna llamada name_area"
                // pero no intentará crearla en la base de datos por el [NotMapped] en la clase.
                entity.Property(e => e.name_area).HasColumnName("name_area"); // solucion al areanombre null 
            });
        }
    }
}
