using AutoMapper;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Director, PersonDTO>()
                .ForMember(dest => dest.IsActor, opt => opt.MapFrom(src => false));

            CreateMap<Actor, PersonDTO>()
                .ForMember(dest => dest.IsActor, opt => opt.MapFrom(src => true));
        }
    }
}
