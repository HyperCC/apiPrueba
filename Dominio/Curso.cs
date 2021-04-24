using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Curso
    {
        // atributos propios de la clase
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public DateTime? FechaCreacion { get; set; }

        // obtener el Modelos relacionados a este curso
        public Precio PrecioPromocion { get; set; }
        public ICollection<Comentario> ComentarioLista { get; set; }
        public ICollection<CursoInstructor> InstructoresLinks { get; set; }

    }
}
