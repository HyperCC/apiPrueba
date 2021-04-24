using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dominio
{
    public class Precio
    {
        // atributos propios de la clase
        public Guid PrecioId { get; set; }

        // longitud del decimal 
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioActual { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Promocion { get; set; }

        // relacionamiento directo hacia un Curso
        public Guid CursoId { get; set; }

        // Obtener todos el curso relacionado a este precio
        public Curso Curso { get; set; }
    }
}
