using ExaminationSystem.BLL.Managers.SqlConnectionStringProviders.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExaminationSystem.BLL.Managers.SqlConnectionStringProviders;

public class SqlConnectionStringProvider : ISqlConnectionStringProvider
{
    private readonly ILogger<SqlConnectionStringProvider> _logger;
    private readonly SqlConnectionStringOptions _options;

    public SqlConnectionStringProvider(IOptions<SqlConnectionStringOptions> options,
        ILogger<SqlConnectionStringProvider> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public string GetSqlDatabaseConnectionString()
    {
        var builder = new SqlConnectionStringBuilder();

        if (string.IsNullOrEmpty(_options.DataSource))
        {
            throw new ArgumentNullException(nameof(_options.DataSource),
                "Data source for connection string can't be null or empty");
        }

        builder.DataSource = _options.DataSource;

        if (string.IsNullOrEmpty(_options.InitialCatalog))
        {
            throw new ArgumentNullException(nameof(_options.InitialCatalog),
                "Initial catalog for connection string can't be null or empty");
        }

        builder.InitialCatalog = _options.InitialCatalog;

        var useWindowsAuthentication = true;

        if (!string.IsNullOrEmpty(_options.UserId) && !string.IsNullOrEmpty(_options.Password))
        {
            builder.UserID = _options.UserId;
            builder.Password = _options.Password;
            useWindowsAuthentication = false;

            _logger.LogDebug($"{nameof(_options.UserId)} and {nameof(_options.Password)} were found in options");
        }

        builder.IntegratedSecurity = useWindowsAuthentication;

        return builder.ToString();
    }
}