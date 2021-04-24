using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    // modelamiento de DB para el sistema, utilizando EF para las migraciones 
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        // debe extender de DbContextOptions para poder configurarse
        public CursosOnlineContext(DbContextOptions options) : base(options)
        {
        }

        // metodo heredado para re configurar relaciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // genera las tablas donde se almacenaran los datos
            base.OnModelCreating(modelBuilder);

            // declaracion de clave primaria compuesta para CursoInstructor
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new { ci.InstructorId, ci.CursoId });
        }

        // conversion de los modelos a entidades en la Solucion
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Precio> Precio { get; set; }
    }
}
