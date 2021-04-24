using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ExcepcionesPersonalizadas
{
    // carpeta con excepciones personalizadas
    // excepcion para las clases Manejdor dentro de la logica de negocios en controladores
    public class ManejadorExcepcion : Exception
    {
        // codigo http a devolver 
        public HttpStatusCode Codigo { get; }
        // mensajes con los errores obtenidos
        public object Errores { get; }

        public ManejadorExcepcion(HttpStatusCode codigo, object errores = null)
        {
            this.Codigo = codigo;
            this.Errores = errores;
        }
    }
}
