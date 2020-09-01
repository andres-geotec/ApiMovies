using AutoMapper;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Mappers
{
    public class MapperMovies: Profile
    {
        public MapperMovies()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, MovieCreateDto>().ReverseMap();
            CreateMap<Movie, MovieUpdateDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}