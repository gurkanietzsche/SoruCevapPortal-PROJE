using AutoMapper;
using SoruCevapPortal.DTOs;
using SoruCevapPortal.Models;

namespace SoruCevapPortal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDTO>();

            // Question mappings
            CreateMap<Question, QuestionDTO>()
                .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.Votes.Sum(v => v.IsUpvote ? 1 : -1)))
                .ForMember(dest => dest.AnswerCount, opt => opt.MapFrom(src => src.Answers.Count));
            CreateMap<QuestionUpdateDTO, Question>();

            CreateMap<QuestionCreateDTO, Question>();

            // Answer mappings
            CreateMap<Answer, AnswerDTO>()
                .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.Votes.Sum(v => v.IsUpvote ? 1 : -1)))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count));
            CreateMap<AnswerCreateDTO, Answer>();
            CreateMap<AnswerUpdateDTO, Answer>();

            // Comment mappings
            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentCreateDTO, Comment>();

            // Tag mappings
            CreateMap<Tag, TagDTO>();
            CreateMap<TagCreateDTO, Tag>();

            // Vote mappings
            CreateMap<VoteDTO, Vote>();
        }
    }
}