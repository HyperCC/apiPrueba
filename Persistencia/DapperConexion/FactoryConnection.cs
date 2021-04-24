using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Persistencia.DapperConexion
{
    // procesar la conexion a la DB mediante procedimientos almacenados
    public class FactoryConnection : IFactoryConnection
    {
        // inyeccion de la cadena de conexion
        private IDbConnection _connection;
        // cadena de conexion; nombre de servidor, nombre DB, credenciales para la DB...
        private readonly IOptions<ConexionConfiguracion> _configs;
        public FactoryConnection(IOptions<ConexionConfiguracion> configs)
        {
            this._configs = configs;
        }

        public void CloseConnection()
        {
            // en caso de esta la conexion abierta, se cierra.
            if (this._connection != null && this._connection.State == ConnectionState.Open)
                this._connection.Close();
        }

        public IDbConnection GetConnection()
        {
            // si no existe una conexion a la DB se crea una.
            if (this._connection == null)
                this._connection = new SqlConnection(this._configs.Value.DefaultConnection);

            // en caso de ue la conexion no este abierta, se debe abrir
            if (this._connection.State != ConnectionState.Open)
                this._connection.Open();

            return this._connection;
        }
    }
}
