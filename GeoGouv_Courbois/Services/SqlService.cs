using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using GeoGouv_Courbois.Configurations;
using GeoGouv_Courbois.Services.Interfaces;

namespace GeoGouv_Courbois.Services
{
    public class SqlService : ISqlService
    {
        private readonly SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
        private readonly ILogger<SqlService> _logger;
        private readonly SqlConfiguration _sqlLoginDatas;
        public SqlService(IOptions<SqlConfiguration> sqlConfiguration, ILogger<SqlService> logger)
        {
            _sqlLoginDatas = sqlConfiguration.Value;

            _builder.DataSource = _sqlLoginDatas.serverUrl;
            _builder.InitialCatalog = _sqlLoginDatas.databaseName;
            _builder.IntegratedSecurity = true;
            _builder.TrustServerCertificate = true;
            _builder.Encrypt = true;
            _builder.ConnectTimeout = 2;

            _logger = logger;
        }

        public bool initConnexion()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
                {
                    connection.Open();
                    _logger.LogInformation("Connexion to SQL DB successfully reached");
                    return true;
                }

            }
            catch (SqlException exception)
            {
                _logger.LogError(exception, "Connexion to SQL DB failed");

                if (exception.Number == 4060)
                {
                    return CheckAndCreateDatabase();
                }

                return false;
            }
        }

        private bool CheckAndCreateDatabase()
        {
            try
            {
                _builder.InitialCatalog = Constants.MASTER_DATABASE;
                using var connection = new SqlConnection(_builder.ConnectionString);
                connection.Open();

                //Vérifie sur la base de donnée existe et si elle n'existe pas on la créée
                string checkDbQuery = @"
                    IF NOT EXISTS (
                        SELECT name 
                        FROM sys.databases 
                        WHERE name = @DatabaseName
                    )
                    BEGIN
                        CREATE DATABASE [" + _sqlLoginDatas.databaseName + @"]
                    END";

                using var command = new SqlCommand(checkDbQuery, connection);
                command.Parameters.AddWithValue("@DatabaseName", _sqlLoginDatas.databaseName);

                command.ExecuteNonQuery();
                _logger.LogInformation("Database created successfully");

                return true;
            }
            catch (SqlException exception)
            {
                //Erreur lors de la tentative de création de la base de donnée
                _logger.LogError(exception, "Error while trying to create database");
                return false;
            }
        }
    }
}
