using AutoMapper;
using StreamingMovie.Application.DTOs;
using StreamingMovie.Domain.Entities;

namespace StreamingMovie.Application.Mappings
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<RatingRequestDTO, Rating>()
                .ForMember(dest => dest.UserId, opt =>
                {
                    opt.PreCondition(src => src.UserId.HasValue);
                    opt.MapFrom(src => src.UserId.Value);
                })
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Rating, RatingResponseDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));
            CreateMap<CommentRequestDTO, Comment>()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Comment, CommentResponseDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl));
        }
    }
}