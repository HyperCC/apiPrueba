using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// controlador de con constructor personalizado para ahorrar codigo en los demas controller
namespace WebApi.Controllers.ControllerPersonalizado
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalControllerBase : ControllerBase
    {
        private IMediator _mediator;

        // en caso de que _mediator es null, su valor será HttpContext.RequestServices.GetService<IMediator>()
        protected IMediator MediadorHerencia => _mediator ??
            (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
