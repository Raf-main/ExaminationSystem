namespace ExaminationSystem.BLL.Managers.SqlConnectionStringProviders;

public interface ISqlConnectionStringProvider
{
    string GetSqlDatabaseConnectionString();
}