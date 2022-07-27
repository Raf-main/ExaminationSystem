using AutoMapper;
using ExaminationSystem.API.DTOs.Request;
using ExaminationSystem.API.DTOs.Response;
using ExaminationSystem.BLL.Models;
using ExaminationSystem.BLL.Models.Create;

namespace ExaminationSystem.API.Mapping;

public class ApiProfile : Profile
{
    public ApiProfile()
    {
        AllowNullCollections = true;

        CreateMap<LoginUserRequest, UserLoginData>();

        CreateMap<LoginResult, LoginResponse>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Tokens.AccessToken));

        CreateMap<LoginResult, TokenResponse>()
            .ForMember(dest => dest.RefreshTokenExpirationDate,
                opt => opt.MapFrom(src => src.Tokens.RefreshTokenExpirationDate))
            .ForMember(dest => dest.AccessToken,
                opt => opt.MapFrom(src => src.Tokens.AccessToken))
            .ForMember(dest => dest.RefreshToken,
                opt => opt.MapFrom(src => src.Tokens.RefreshToken));

        CreateMap<UserTokens, TokenResponse>();

        CreateMap<RegisterUserRequest, UserCreateData>();

        CreateMap<RefreshAccessRequest, UserTokens>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.ExpiredToken));
        CreateMap<UserTokens, TokenResponse>();

        CreateMap<CreateRoomMessageRequest, RoomMessageCreateData>();

        CreateMap<RoomMessage, RoomMessageResponse>()
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.Message.CreatedOn))
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.Message.Id))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Message.Text));

        CreateMap<CreateRoomRequest, RoomCreateData>();
        CreateMap<Room, RoomResponse>();

        CreateMap<Invite, RoomInviteResponse>()
            .ForMember(dest => dest.FromEmail, opt => opt.MapFrom(src => src.From.Email))
            .ForMember(dest => dest.FromId, opt => opt.MapFrom(src => src.From.Id))
            .ForMember(dest => dest.ToEmail, opt => opt.MapFrom(src => src.To.Email))
            .ForMember(dest => dest.ToId, opt => opt.MapFrom(src => src.To.Id))
            .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room.Id));

        CreateMap<CreateExamRequest, ExamCreateData>();
        CreateMap<Exam, ExamResponse>();

        CreateMap<CreateMultipleQuestionRequest, MultipleQuestionCreateData>();
        CreateMap<MultipleQuestion, MultipleQuestionResponse>();

        CreateMap<CreateMultipleQuestionOptionRequest, MultipleQuestionOptionCreateData>();
        CreateMap<MultipleQuestionOption, MultipleQuestionOptionResponse>();

        CreateMap<CreateOpenQuestionRequest, OpenQuestionCreateData>();
        CreateMap<OpenQuestion, OpenQuestionResponse>();

        CreateMap<CreateOpenQuestionOptionRequest, OpenQuestionOptionCreateData>();
        CreateMap<OpenQuestionOption, OpenQuestionOptionResponse>();
    }
}