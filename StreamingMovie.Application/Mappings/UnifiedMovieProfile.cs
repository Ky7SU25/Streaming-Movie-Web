using AutoMapper;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Mappings
{
    public class UnifiedMovieProfile : Profile
    {
        public UnifiedMovieProfile()
        {
            // UnifiedMovie mappings
            CreateMap<UnifiedMovie, UnifiedMovieDTO>();

            CreateMap<Series, UnifiedMovieDTO>()
                   .ForMember(dest => dest.IsSeries, opt => opt.MapFrom(src => true));

            CreateMap<Movie, UnifiedMovieDTO>()
                   .ForMember(dest => dest.IsSeries, opt => opt.MapFrom(src => false));

            // UnifiedMovie to MovieDetailDTO mappings
            CreateMap<UnifiedMovie, MovieDetailDTO>();

            CreateMap<Series, MovieDetailDTO>()
                   .IncludeBase<Series, UnifiedMovieDTO>()
                   .ForMember(dest => dest.IsSeries, opt => opt.MapFrom(src => true));

            CreateMap<Movie, MovieDetailDTO>()
                   .IncludeBase<Movie, UnifiedMovieDTO>()
                   .ForMember(dest => dest.IsSeries, opt => opt.MapFrom(src => false));
        }
    }
}
