using Aplicacion.ExcepcionesPersonalizadas;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class BuscarPorId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // buscar un Instructor mediante una ID mediante los procedimientos almacenados
                var instructor = await this._instructorRepository.ObtenerPorId(request.id);

                // verificar si se encontro el Instructor por el ID
                if (instructor == null)
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "no se encontro el instructor" });

                return (InstructorModel)instructor;
            }
        }
    }
}
