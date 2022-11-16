using AutoMapper;
using PeliculaApi.Models;
using PeliculaApi.Models.Dtos;

namespace PeliculaApi.PeliculaMapper;

public class PeliculaMappers : Profile
{
    public PeliculaMappers()
    {
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaCreateDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaUpdateDto>().ReverseMap();
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
        CreateMap<Usuario, UsuarioAuthDto>().ReverseMap();
        CreateMap<Usuario, UsuarioAuthLoginDto>().ReverseMap();
    }
}