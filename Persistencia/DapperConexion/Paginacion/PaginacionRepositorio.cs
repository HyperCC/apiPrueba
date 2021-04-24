using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    // implmentacion de la interface para operar con las acciones de Paginacion
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConnection _factoryConnection;
        public PaginacionRepositorio(IFactoryConnection factoryConnection)
        {
            this._factoryConnection = factoryConnection;
        }

        public async Task<PaginacionModel> DevolverPaginacion(string storeProcedure, int numeroPagina, int cantidadElementos, IDictionary<string, object> parametrosFiltro, string ordenamientoColumnas)
        {
            // nueva paginacio vacia
            PaginacionModel paginacionModel = new PaginacionModel();
            // datos a guardar desde la DB
            List<IDictionary<string, object>> listaReporte = null;

            int totalRecords = 0;
            int totalPaginas = 0;

            try
            {
                var conexion = this._factoryConnection.GetConnection();
                // Parametros de entrada. parametros para saber exactamente ue devolver.
                DynamicParameters parametros = new DynamicParameters();

                foreach (var param in parametrosFiltro)
                {
                    // se agregan cada paramtro y su valor respectivo
                    parametros.Add("@" + param.Key, param.Value);
                }

                // parametros de entrada. Detectar ue se debe devolver
                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumnas);

                // Parametros de Salida. Se agrega el nombre del dato, el valor del dato, el tipo del dato y se identifia si se Input u Output.
                parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                // obtener los datos mendiante procedimiento almacenado
                var result = await conexion.QueryAsync(storeProcedure, null, commandType: CommandType.StoredProcedure);

                // convertir cada dato en "result" a objetos de tipo IDictionary
                listaReporte = result.Select(x => (IDictionary<string, object>)x).ToList();
                // reasignar la pagina como una nueva pagina con los datos de paginas anteriores
                paginacionModel.ListaRecords = listaReporte;
                paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo devolver la paginacion - procedimiento almacenadi - ", ex);
            }
            finally
            {
                this._factoryConnection.CloseConnection();
            }

            // se devuelve una instancia con los datos ya procesados 
            return paginacionModel;
        }
    }
}
