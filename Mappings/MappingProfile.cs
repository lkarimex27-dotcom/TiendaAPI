using AutoMapper;
using LaTiendaAPI.Models;
using LaTiendaAPI.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LaTiendaAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo para listar u obtener un producto (incluye el nombre de la categoría)
            CreateMap<Producto, ProductoDto>().ForMember(dest => dest.CategoriaNombre,
                           opt => opt.MapFrom(src => src.objCategoria != null ? src.objCategoria.nombre : string.Empty));

            // Dentro de tu MappingProfile()
            CreateMap<ProductoCreateDto, Producto>();
            // Mapeo para Categorías
            CreateMap<Categoria, CategoriaDto>();
        }
    }
}