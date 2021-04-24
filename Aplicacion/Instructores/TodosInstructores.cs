using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    // resuelve la logica declarada en persistencia para los procedimientos almacenados
    public class TodosInstructores
    {
        // clase para identifficar la lista para instructorModel directamente relacionado a la implementacion de los procedimeintos almacenados
        public class Lista : IRequest<List<InstructorModel>> { }

        // logica de resolucion de consultas en procedimientos almacenados
        public class Manejador : IRequestHandler<Lista, List<InstructorModel>>
        {
            // contiene toda la implementacion para los metodos resueltos para los procedimientos almacenados
            private readonly InstructorRepositorio _instructorRepositorio;
            public Manejador(InstructorRepositorio instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }

            public async Task<List<InstructorModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                // todos los instructores obtenidos por procedimientos almacenados
                var result = await this._instructorRepositorio.ObtenerLista();
                return result.ToList();
            }
        }
    }
}
