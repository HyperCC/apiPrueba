using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Instructor
    {
        // atributos propios de la clase
        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
        public DateTime? FechaCreacion { get; set; }

        // obtener todos los cursos relacionados a este instructor
        public ICollection<CursoInstructor> CursoLink { get; set; }
    }
}
