using Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Contratos
{
    public interface IJwtGenerador
    {
        string CreateToken(Usuario usuario, List<string> roles);
    }
}
