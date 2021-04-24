using Aplicacion.Contratos;
using Dominio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    // generador de tokens como strings
    public class JwtGenerador : IJwtGenerador
    {
        public string CreateToken(Usuario usuario, List<string> roles)
        {
            // los claims son los datos de los usuarios que se quieren compartir con los clientes.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            // verifiacar que los roles pasados no sean nulos
            if (roles != null)
            {
                foreach (var rol in roles)
                {
                    // agregar los roles como claims dentro del token
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }

            // credenciales de acceso para el usuario
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // generar el token, su duracion y las credenciales de acceso
            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credenciales
            };

            // creacion de la instancia del token como tal
            var tokenManejador = new JwtSecurityTokenHandler();
            var token = tokenManejador.CreateToken(tokenDescripcion);

            // devuelve el string con el token generado.
            return tokenManejador.WriteToken(token);
        }
    }
}
