using AutoMapper;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;
using ExaminationSystem.DAL.Entities;
using Org.BouncyCastle.Crypto;

namespace ExaminationSystem.BLL.Mapping;

public class BllProfile : Profile
{
    public BllProfile()
    {
        AllowNullCollections = true;

        CreateMap<ApplicationUser, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));

        CreateMap<UserRefreshToken, RefreshTokenDbEntry>()
            .ForMember(dest => dest.UtcExpirationTime, opt =>
                opt.MapFrom(src => src.UtcExpirationDate));

        CreateMap<RoomMessageDbEntry, RoomMessage>();
        CreateMap<MessageDbEntry, Message>();

        CreateMap<RoomMessageCreateData, RoomMessageDbEntry>()
            .ForPath(dest => dest.Message.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
            .ForPath(dest => dest.Message.Text, opt => opt.MapFrom(src => src.Text));

        CreateMap<RoomCreateData, RoomDbEntry>();
        CreateMap<RoomDbEntry, Room>();

        CreateMap<RoomInviteDbEntry, Invite>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
            .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To))
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room));

        CreateMap<ExamCreateData, ExamDbEntry>();
        CreateMap<ExamDbEntry, Exam>();

        CreateMap<MultipleQuestionCreateData, MultipleQuestionDbEntry>();
        CreateMap<MultipleQuestionDbEntry, MultipleQuestion>();

        CreateMap<MultipleQuestionOptionCreateData, MultipleQuestionOptionDbEntry>();
        CreateMap<MultipleQuestionOptionDbEntry, MultipleQuestionOption>();

        CreateMap<OpenQuestionCreateData, OpenQuestionDbEntry>();
        CreateMap<OpenQuestionDbEntry, OpenQuestion>();

        CreateMap<OpenQuestionOptionCreateData, OpenQuestionOptionDbEntry>();
        CreateMap<OpenQuestionOptionDbEntry, OpenQuestionOption>();
    }
}