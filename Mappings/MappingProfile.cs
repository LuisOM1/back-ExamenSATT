using AutoMapper;
using ExamenSATT.DTOs;
using ExamenSATT.Models;

namespace ExamenSATT.Mappings
{
    // Clase que define las reglas de cómo pasar del Modelo (BD) al DTO (Cliente)
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Origen, Destino>()
            CreateMap<EmpleadoModel, EmpleadoDTO>()
                // Si los nombres son diferentes, los mapeamos manualmente:
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id_empl))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.nomb_empl} {src.ape_empl}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.emai_empl ?? "Sin correo"))
                .ForMember(dest => dest.Sueldo, opt => opt.MapFrom(src => src.suel_empl))
                .ForMember(dest => dest.AreaNombre, opt => opt.MapFrom(src => src.name_area))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.esta == "A" ? "Activo" : "Inactivo"));

            CreateMap<EmpleadoCreateUpdateDTO, EmpleadoModel>()
                .ForMember(m => m.id_area, d => d.MapFrom(s => s.IdArea))
                .ForMember(m => m.nomb_empl, d => d.MapFrom(s => s.NombEmpl))
                .ForMember(m => m.ape_empl, d => d.MapFrom(s => s.ApeEmpl))
                .ForMember(m => m.emai_empl, d => d.MapFrom(s => s.EmaiEmpl))
                .ForMember(m => m.suel_empl, d => d.MapFrom(s => s.SuelEmpl))
                .ForMember(m => m.fech_ingr, d => d.MapFrom(s => s.FechIngr));
        }
    }
}
