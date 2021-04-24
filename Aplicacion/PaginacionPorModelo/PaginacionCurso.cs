using MediatR;
using Persistencia.DapperConexion.Paginacion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.PaginacionPorModelo
{
    // implementacion del repositorio para paginar con Cursos
    public class PaginacionCurso
    {
        public class Ejecuta : IRequest<PaginacionModel>
        {
            // se filtrara por el titulo del curso de ser establecido, en este caso filtra por titulo
            public string Titulo { get; set; }
            // el resto de parametros son para determinar proximas paginaciones
            public int NumeroPagina { get; set; }
            public int CantidadElementos { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {
            private readonly IPaginacion _paginacion;
            public Manejador(IPaginacion paginacion)
            {
                this._paginacion = paginacion;
            }

            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // procedimiento almacenado para devolver la paginacion
                var storeProcedure = "usp_obtener_curso_paginacion";
                // columna por la cual se ordenara la paginacion, en este caso por Titulo(columna en DB)
                var ordenamiento = "Titulo";

                // declaracion de los parametros
                var parametros = new Dictionary<string, object>();
                parametros.Add("NombreCurso", request.Titulo);

                // se devuelve la paginacion dentro de la capa de negocios
                return await this._paginacion.DevolverPaginacion(storeProcedure,
                    request.NumeroPagina,
                    request.CantidadElementos,
                    parametros,
                    ordenamiento);
            }
        }
    }
}
