using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class CursoInstructor
    {
        // relacionamientos entre Instructores y Cursos
        public Guid InstructorId { get; set; }
        public Guid CursoId { get; set; }

        // Obtener los modelos de esta relacion
        public Curso Curso { get; set; }
        public Instructor Instructor { get; set; }
    }
}
