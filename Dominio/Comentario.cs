using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Comentario
    {
        // atributos propios de la clase
        public Guid ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public DateTime? FechaCreacion { get; set; }

        // relacionamiento directo hacia un Curso
        public Guid CursoId { get; set; }

        // Obtener el curso relacionado a este precio
        public Curso Curso { get; set; }
    }
}
