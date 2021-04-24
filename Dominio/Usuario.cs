using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    // usuario que ingrese y loguee con Core Identity debe extender de IdentityUser heredando atributos
    public class Usuario : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}
