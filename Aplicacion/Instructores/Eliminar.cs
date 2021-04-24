using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // resultado de la logica para la eliminacion en InstructorRepositorio 
                var resultado = await this._instructorRepository.Eliminar(request.id);

                // segun los resultados de Eliminar en InstructorRepositorio se arroja un resultado externo
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el Instructor - procedimiento almacenado ELIMINAR - ");
            }
        }
    }
}
