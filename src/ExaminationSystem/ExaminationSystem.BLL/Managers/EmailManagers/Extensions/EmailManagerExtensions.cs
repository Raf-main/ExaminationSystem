using ExaminationSystem.BLL.Managers.EmailManagers.Configuration;
using ExaminationSystem.BLL.Managers.EmailManagers.Options;
using Microsoft.Extensions.Configuration;

namespace ExaminationSystem.BLL.Managers.EmailManagers.Extensions;

public static class EmailManagerExtensions
{
    public static void BuildEmailOptions(this IConfiguration configuration,
        EmailOptions options)
    {
        options.From = configuration[EmailConfigurationKeys.From];
        options.SmtpServer = configuration[EmailConfigurationKeys.SmtpServer];
        options.UserName = configuration[EmailConfigurationKeys.UserName];
        options.Password = configuration[EmailConfigurationKeys.Password];

        if (int.TryParse(configuration[EmailConfigurationKeys.Port], out var port))
        {
            options.Port = port;
        }
        else
        {
            throw new FormatException("Email port has incorrect format");
        }
    }
}