using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using GeoGouv_Courbois.Configurations;
using GeoGouv_Courbois.Services.Interfaces;
using System.Data;

namespace GeoGouv_Courbois.Services
{
    public class SqlService : ISqlService
    {
        private readonly SqlConnectionStringBuilder _builder;
        private readonly ILogger<SqlService> _logger;

        public SqlService(IOptions<SqlConfiguration> sqlConfiguration, ILogger<SqlService> logger)
        {
            var _sqlLoginDatas = sqlConfiguration.Value;

            _builder = new SqlConnectionStringBuilder
            {
                DataSource = _sqlLoginDatas.serverUrl,
                InitialCatalog = _sqlLoginDatas.databaseName,
                IntegratedSecurity = true,
                TrustServerCertificate = true,
                Encrypt = true
            };

            _logger = logger;
        }

        //Connexion à ma base de données et insertion via procédure stockée des paramètres avec protection contre injection SQL 
        public bool InsertVilleAndDepartement(string codePostalDepartement, string nomDepartement,
                                     string commune, string nomCommuneMinuscule,
                                     string codePostal, string codeInsee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.InsertVilleAndDepartement", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CodePostalDepartement", codePostalDepartement);
                        command.Parameters.AddWithValue("@NomDepartement", nomDepartement);
                        command.Parameters.AddWithValue("@Commune", commune);
                        command.Parameters.AddWithValue("@NomCommuneMinuscule", nomCommuneMinuscule);
                        command.Parameters.AddWithValue("@CodePostal", codePostal);
                        command.Parameters.AddWithValue("@CodeInsee", codeInsee);

                        command.ExecuteNonQuery();

                        _logger.LogInformation("Insert into ville and departement succeed.");
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error while trying to insert datas.");
                return false;
            }
        }
        //Connexion à ma base de données et récupération via procédure stockée des villes et départements
        public List<Dictionary<string, object>> GetAllDepartementsWithVilles()
        {
            var departements = new List<Dictionary<string, object>>();
            var departementDict = new Dictionary<string, Dictionary<string, object>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.GetAllDepartementsWithVilles", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string codeDepartement = reader["CodePostalDepartement"].ToString();

                                if (!departementDict.ContainsKey(codeDepartement))
                                {
                                    departementDict[codeDepartement] = new Dictionary<string, object>
                            {
                                { "CodePostalDepartement", codeDepartement },
                                { "NomDepartement", reader["NomDepartement"].ToString() },
                                { "Villes", new List<Dictionary<string, object>>() }
                            };
                                }

                                if (reader["CodeVilleCodePostalInsee"] != DBNull.Value)
                                {
                                    var ville = new Dictionary<string, object>
                            {
                                { "CodeVilleCodePostalInsee", reader["CodeVilleCodePostalInsee"] },
                                { "Commune", reader["Commune"].ToString() },
                                { "NomCommuneMinuscule", reader["NomCommuneMinuscule"].ToString() },
                                { "CodePostal", reader["CodePostal"].ToString() },
                                { "CodeInsee", reader["CodeInsee"].ToString() }
                            };

                                    ((List<Dictionary<string, object>>)departementDict[codeDepartement]["Villes"]).Add(ville);
                                }
                            }
                        }
                    }
                }

                departements = departementDict.Values.ToList();
                _logger.LogInformation("Récupération des départements et villes réussie.");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des départements et villes.");
            }

            return departements;
        }


    }
}
