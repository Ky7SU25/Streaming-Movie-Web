using AutoMapper;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Mappings
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Rating, RatingRequestDTO>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Rating, RatingResponseDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));

            CreateMap<Comment, CommentRequestDTO>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Comment, CommentResponseDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));
        }
    }
}