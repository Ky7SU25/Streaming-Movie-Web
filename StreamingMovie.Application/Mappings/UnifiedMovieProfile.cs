using AutoMapper;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Mappings
{
    public class UnifiedMovieProfile : Profile
    {
        public UnifiedMovieProfile()
        {
            CreateMap<UnifiedMovie, UnifiedMovieDTO>();
        }
    }
}
