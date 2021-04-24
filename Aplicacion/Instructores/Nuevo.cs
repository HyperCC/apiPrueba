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
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
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
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }

            // ejecuta la creacion de un nuevo Instructor por procedimientos almacenados 
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var result = await this._instructorRepository.Nuevo(
                    request.Nombre,
                    request.Apellidos,
                    request.Titulo
                    );

                if (result >= 0)
                    return Unit.Value;

                throw new Exception("No se pudo crear el instructor - procedimiento almacenado NUEVO - ");
            }
        }
    }
}
