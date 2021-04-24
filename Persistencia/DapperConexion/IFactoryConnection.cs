using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Persistencia.DapperConexion
{
    // operaciones para la conexion con al DB
    public interface IFactoryConnection
    {
        // cerrar las conexiones existentes con la DB
        void CloseConnection();

        // obtener el objeto de conexion
        IDbConnection GetConnection();
    }
}
