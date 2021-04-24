using Aplicacion.ExcepcionesPersonalizadas;
using Dominio;
using FluentValidation;
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
    // editar un curso con todos los parametros
    public class EditarCurso
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
        }

        // validaciones a los parametros entregados
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            // instancia de la DB
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // obtener el curso a editar
                var curso = await this._context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El curso buscado no existe" });

                // actualizacion del curso como tal 
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                curso.FechaCreacion = DateTime.UtcNow;

                // buscar el precio para el curso dado
                var precioEntidad = this._context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if (precioEntidad != null)
                {
                    // actualizacion de precio y promocion para el curso encontrado
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                }
                else
                {
                    // si no hay preio se genera uno nuevo de valor 0
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };
                    await this._context.Precio.AddAsync(precioEntidad);
                }

                // verificar si se enviaron nuevas relacines entre curso e instructor
                if (request.ListaInstructor.Count > 0)
                {
                    var instructoresDB = this._context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                    // para borran todos los instructores relacionados a un curso
                    foreach (var instructorEliminar in instructoresDB)
                        this._context.CursoInstructor.Remove(instructorEliminar);

                    // agregar los nuevos instructores relacionados a un curso proveidos por el request
                    foreach (var idInstructor in request.ListaInstructor)
                    {
                        var nuevoInstructor = new CursoInstructor
                        {
                            CursoId = request.CursoId,
                            InstructorId = idInstructor
                        };
                        this._context.CursoInstructor.Add(nuevoInstructor);
                    }
                }

                // verificar si la operacion de guardar los datos es completamente satisfactoria
                var result = await this._context.SaveChangesAsync();
                if (result > 0)
                    return Unit.Value;

                throw new Exception("No se pudieron guardar los cambios en el curso");
            }
        }
    }
}
