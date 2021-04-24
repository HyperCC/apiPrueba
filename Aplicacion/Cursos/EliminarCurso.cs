using Aplicacion.ExcepcionesPersonalizadas;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// el directorio Cursos indica actividades para con el modelo Curso
namespace Aplicacion.Cursos
{
    // eliminar un curso de la DB mediante el ID
    public class EliminarCurso
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            // recordar eliminar las referencias a el curso por eliminar
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // obtener los instructores relacionados a el curso por eliminar
                var instructoresDB = this._context.CursoInstructor.Where(x => x.CursoId == request.Id);
                // eliminar las relaciones 
                foreach (var instructor in instructoresDB)
                    this._context.Remove(instructor);

                // obtener los comentarios de la DB para eliminarlos
                var comentariosDB = this._context.Comentario.Where(x => x.CursoId == request.Id);
                foreach (var comment in comentariosDB)
                    this._context.Comentario.Remove(comment);

                // eliminar el precio asociado al curso 
                var precioDB = this._context.Precio.Where(x => x.CursoId == request.Id).FirstOrDefault();
                if (precioDB != null)
                    this._context.Precio.Remove(precioDB);

                // buscar el curso segun el request del cliente
                var curso = await this._context.Curso.FindAsync(request.Id);
                if (curso == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se puede eliminar el curso - curso no encontrado" });

                // de encontrarse se borrara de la DB 
                this._context.Remove(curso);
                var result = await this._context.SaveChangesAsync();

                // en caso se ser exitoso el borrado 
                if (result > 0)
                    return Unit.Value;

                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}
