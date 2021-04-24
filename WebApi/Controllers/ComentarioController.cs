using Aplicacion.Comentarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.ControllerPersonalizado;

namespace WebApi.Controllers
{
    public class ComentarioController : PersonalControllerBase  
    {
        // se accede mediante http://host:puerto/api/Comentario enviando los atributos respectivos para un nuevo Comentario
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await MediadorHerencia.Send(data);
        }

        // se accede mediante http://host:puerto/api/Comentario/{id} unicamente se agrega el ID
        // y respondera si no encontro el comentaio, si no se pudo eliminar, o codigo 200 si todo fue correcto.
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await MediadorHerencia.Send(new Eliminar.Ejecuta { Id = id });
        }
    }
}
