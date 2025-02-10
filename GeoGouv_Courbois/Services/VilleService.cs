using GeoGouv_Courbois.Controllers;
using GeoGouv_Courbois.Helpers;
using GeoGouv_Courbois.Models;
using GeoGouv_Courbois.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text.Json;

namespace GeoGouv_Courbois.Services
{
    public class VilleService : IVilleService
    {
        private HttpClient _httpClient;
        private ILogger<VilleService> _logger;
        private readonly StringNormalizer _stringNormalizer;
        private List<Ville> _allVilles;
        private ISqlService _sqlService;

        public VilleService(HttpClient httpClient, ILogger<VilleService> logger, StringNormalizer stringNormalizer, ISqlService sqlService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _stringNormalizer = stringNormalizer;
            _sqlService = sqlService;
        }

        //Initialisation de la connexion à l'API GeoGouv récupération des villes et sérialisation des données pour les obtenirs sous forme de liste d'objets "Ville"
        public async Task<List<Ville>> GetVilles()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("communes");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erreur lors de la récupération des Villes");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Ville>>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des Villes");
                return null;
            }
        }

        //Permet de récupérer les villes triées et gère l'affichage par "page" 50 par 50.
        public async Task<IEnumerable<Ville>> GetOrdereVille(int page, int pageSize, string sortBy, bool? ascending, string search)
        {
            if (_allVilles.IsNullOrEmpty())
            {
                _allVilles = await GetVilles();
            }

            _allVilles = SearchVille(search);

            _allVilles = SortVilles(sortBy, ascending);

            return _allVilles
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        //Permet d'insérer en base de donnée la ville et son département via une procédure stockée
        public bool InsertVilleAndDepartementInDB(Departement dpt, Ville ville)
        {

            var departements = _sqlService.GetAllDepartementsWithVilles();

            foreach (var departement in departements)
            {
                Console.WriteLine($"Département : {departement["NomDepartement"]} ({departement["CodePostalDepartement"]})");

                var villes = (List<Dictionary<string, object>>)departement["Villes"];

                foreach (var villet in villes)
                {
                    Console.WriteLine($"   - {villet["Commune"]} ({villet["CodePostal"]})");
                }
            }

            return _sqlService.InsertVilleAndDepartement(dpt.Code, dpt.Nom, ville.Nom, ville.Nom.ToLower(), ville.CodesPostaux.FirstOrDefault(), ville.Code);
        }
        //Récupére les détails d'une ville
        public async Task<Ville> GetVilleDetails(String codeVille)
        {
            return await GetApiData<Ville>("communes/" + codeVille);
        }

        //Récupére les détails d'un département
        public async Task<Departement> GetDepartementDetails(String codeDepartement)
        {
            return await GetApiData<Departement>("departements/" + codeDepartement);
        }

        //Appel à la généricitée afin de pouvoir requêter sur l'api geoGouv en définissant l'objet de retour voulu en fonction du endpoint sur l'api
        private async Task<T> GetApiData<T>(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Erreur lors de la récupération des données pour {endpoint}");
                    return default;
                }

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de l'appel API pour {endpoint}");
                return default;
            }
        }


        //Gère le retour de la requête en cas de recherche 
        private List<Ville> SearchVille(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string normalizedSearch = _stringNormalizer.NormalizeString(search);

                return _allVilles = _allVilles.Where(c =>
                    c.Nom != null && _stringNormalizer.NormalizeString(c.Nom).Contains(normalizedSearch) ||
                    c.Code != null && c.Code.Contains(search) ||
                    c.CodeDepartement != null && c.CodeDepartement.Contains(search) ||
                    c.CodesPostaux != null && c.CodesPostaux.Any(cp => cp.Contains(search))).ToList();
            }
            {
                return _allVilles;
            }
        }

        //Permet de trier les villes en fonction de du critère par ordre asc et desc
        private List<Ville> SortVilles(string sortBy, bool? ascending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return _allVilles.ToList();
            }


            PropertyInfo property = GetPropertyValue(sortBy);

            if (property == null || !ascending.HasValue)
            {
                return _allVilles.ToList();
            }

            return ascending.Value
                ? _allVilles.OrderBy(c => property.GetValue(c, null)).ToList()
                : _allVilles.OrderByDescending(c => property.GetValue(c, null)).ToList();
        }

        //Me retourne via la reflexion la valeur de la propriété de mon objet en comparant les noms sans tenir compte des MAJ etc...
        private PropertyInfo GetPropertyValue(string propertyName)
        {
            return typeof(Ville).GetProperties()
                .FirstOrDefault(property => string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
