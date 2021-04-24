using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            // unicos parametros para editar un Instructor
            public Guid InstructorId;
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;
            public Manejador(IInstructor instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }

            // valida que se ejecuto correctamente la validacion 
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // ejecutar procedimiento almacenado Editar
                var resultado = await this._instructorRepositorio.Editar(request.InstructorId, request.Nombre, request.Apellidos, request.Titulo);

                // verificar si la edicion fue exitosa, resultado debe devolver la cantidad de filas afectadas
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar la data del Instructor - procedimiento almacenado EDITAR - ");
            }
        }
    }
}
