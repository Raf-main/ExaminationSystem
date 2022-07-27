using ExaminationSystem.BLL.Managers.SqlConnectionStringProviders.Options;
using Microsoft.Extensions.Configuration;

namespace ExaminationSystem.BLL.Managers.SqlConnectionStringProviders.Extensions;

public static class EmailManagerExtensions
{
    public static void BuildSqlConnectionStringOptions(this IConfiguration configuration,
        SqlConnectionStringOptions options)
    {
        options.DataSource = configuration[SqlConnectionConfigurationKeys.DataSource];
        options.InitialCatalog = configuration[SqlConnectionConfigurationKeys.InitialCatalog];
        options.UserId = configuration[SqlConnectionConfigurationKeys.UserId];
        options.Password = configuration[SqlConnectionConfigurationKeys.Password];
    }
}