using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            // datos necesarios para crear un nuevo comentario
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string Comentario { get; set; }
            public Guid CursoId { get; set; }
        }

        // las validaciones de datos para Ejecuta 
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.Puntaje).NotEmpty();
                RuleFor(x => x.Comentario).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            // conexion a la DB 
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // creacion de comentario 
                var comentario = new Comentario
                {
                    ComentarioId = Guid.NewGuid(),
                    Alumno = request.Alumno,
                    ComentarioTexto = request.Comentario,
                    CursoId = request.CursoId,
                    FechaCreacion = DateTime.UtcNow
                };

                // agregarlo a la DB
                this._context.Comentario.Add(comentario);
                var resultado = await this._context.SaveChangesAsync();

                // verificar los posibles resultados de la operacion ADD()
                if (resultado > 0)
                    return Unit.Value;

                throw new Exception("No se pudo insertar el Comentario - nuevo EF CORE - ");
            }
        }
    }
}
