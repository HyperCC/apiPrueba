using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    // contratos para la paginacion
    public interface IPaginacion
    {
        // devolver lista de modelos ya paginados
        Task<PaginacionModel> DevolverPaginacion(
            string storeProcedure,
            int numeroPagina,
            int cantidadElementos,

            // parametros para el filtro de la busueda
            IDictionary<string, object> parametrosFiltro,
            string ordenamientoColumnas
            );
    }
}
