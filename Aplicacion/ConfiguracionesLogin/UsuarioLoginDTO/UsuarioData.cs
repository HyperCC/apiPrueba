using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ConfiguracionesLogin.UsuarioLoginDTO
{
    // clase en funcion de DTO para devolver unicamente datos necesarios
    // para despues del login
    public class UsuarioData
    {
        public string NombreCompleto { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Imagen { get; set; }
    }
}
