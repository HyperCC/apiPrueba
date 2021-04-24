using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.DapperConexion
{
    // toma una instancia de la cadena de conexion en appsettings.json
    public class ConexionConfiguracion
    {
        // tiene el valor como tal de la cadena de conexion
        public string DefaultConnection { get; set; }
    }
}
