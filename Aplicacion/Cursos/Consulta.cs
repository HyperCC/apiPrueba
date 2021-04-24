using AutoMapper;
using Dominio;
using Dominio.ModelosDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// el directorio Cursos indica actividades para con el modelo Curso
namespace Aplicacion.Cursos
{
    // obtener todos los cursos guardados en la DB
    public class Consulta
    {
        public class CursosTodos : IRequest<List<CursoDto>>
        { }

        public class Manejador : IRequestHandler<CursosTodos, List<CursoDto>>
        {
            // propiedades iniciales para la DB y el mapeo de modelos 
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<List<CursoDto>> Handle(CursosTodos request, CancellationToken cancellationToken)
            {
                // devolver los Cursos junto a los Comentarios, el Precio, y los Instructores relacionados a un curso
                var cursos = await this._context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructoresLinks)
                    .ThenInclude(x => x.Instructor).ToListAsync();

                // recibe el tipo de dato origen, y el segundo es para saber en que se convertira.
                var cursosDTO = this._mapper.Map<List<Curso>, List<CursoDto>>(cursos);

                // debe hcerse el mapping para obtener los instructores desde ClaseDTO
                return cursosDTO;
            }
        }
    }
}
