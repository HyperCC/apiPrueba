using AutoMapper;
using Dominio;
using Dominio.ModelosDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplicacion
{
    // clase encargada de manejar mapeos entre modelo comun y DTO
    public class MappingProfile: Profile 
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDto>()
                // toda esta linea ayuda a obtener la lista de instructores para un curso a travez de ClaseDTO
                .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructoresLinks.Select(a => a.Instructor).ToList()))
                // obtener toos los comentarios asociados a el curso
                .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                // obtener el precio del curso
                .ForMember(x => x.Precio, y => y.MapFrom(z => z.PrecioPromocion));

            // mapeos entre modelos y DTO
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();
        }
    }
}
