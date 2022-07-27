using ExaminationSystem.BLL.Helpers;
using ExaminationSystem.BLL.Managers.AccountManagers;
using ExaminationSystem.BLL.Managers.EmailManagers;
using ExaminationSystem.BLL.Managers.ExamManagers;
using ExaminationSystem.BLL.Managers.FileManagers;
using ExaminationSystem.BLL.Managers.QuestionManagers;
using ExaminationSystem.BLL.Managers.QuestionManagers.Abstractions;
using ExaminationSystem.BLL.Managers.RoomManagers;
using ExaminationSystem.BLL.Managers.SqlConnectionStringProviders;
using ExaminationSystem.BLL.Managers.TokenManagers;
using ExaminationSystem.BLL.Managers.TokenManagers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExaminationSystem.BLL.Extensions;

public static class BllServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddSingleton<ITemplateManager, TemplateManager>();
        services.AddSingleton<IEmailManager, EmailManager>();
        services.AddSingleton<IFileManager, FileManager>();
        services.AddSingleton<UrlHelper>();

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<ISqlConnectionStringProvider, SqlConnectionStringProvider>();
        services.AddTransient<IRoomManager, RoomManager>();
        services.AddTransient<IRoomInviteManager, RoomInviteManager>();
        services.AddTransient<IRoomMessageManager, RoomMessageManager>();
        services.AddScoped<IAccountManager, AccountManager>();
        services.AddTransient<ITokenManager, JwtTokenManager>();
        services.AddTransient<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddTransient<IAccessTokenGenerator, JwtAccessTokenGenerator>();
        services.AddTransient<IExamManager, ExamManager>();
        services.AddTransient<IMultipleQuestionManager, MultipleQuestionManager>();
        services.AddTransient<IExamResultManager, ExamResultManager>();

        return services;
    }
}