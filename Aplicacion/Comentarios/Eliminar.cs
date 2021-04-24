using Aplicacion.ExcepcionesPersonalizadas;
using MediatR;
using Persistencia;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Comentarios
{
    // eliminar un comentario
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            // buscar un comentario por el ID
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // buscar el comentario mediante la ORM
                var comentario = await this._context.Comentario.FindAsync(request.Id);

                //verificar que el comentario exista
                if (comentario == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el Comentario para eliminar" });

                // eliminar el comentario despues de haberse encontrado
                this._context.Remove(comentario);
                var result = await this._context.SaveChangesAsync();

                // analizar el resultado de eliminacion y guardado de cambios para el comentario eliminado
                if (result > 0)
                    return Unit.Value;

                throw new Exception("No se pudo eliminar el Comentario - eliminar EF CORE - ");
            }
        }
    }
}
