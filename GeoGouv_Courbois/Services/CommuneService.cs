using GeoGouv_Courbois.Helpers;
using GeoGouv_Courbois.Models;
using GeoGouv_Courbois.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text.Json;

namespace GeoGouv_Courbois.Services
{
    public class CommuneService : ICommuneService
    {
        private HttpClient _httpClient;
        private ILogger<CommuneService> _logger;
        private readonly StringNormalizer _stringNormalizer;
        private List<Commune> _allCommunes;

        public CommuneService(HttpClient httpClient, ILogger<CommuneService> logger, StringNormalizer stringNormalizer)
        {
            _httpClient = httpClient;
            _logger = logger;
            _stringNormalizer = stringNormalizer;
        }

        //Initialisation de la connexion à l'API GeoGouv récupération des communes et sérialisation des données pour les obtenirs
        //sous forme de liste d'objets "CommuneViewModel"
        public async Task<List<Commune>> GetCommunes()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("communes");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erreur lors de la récupération des communes");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Commune>>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des communes");
                return null;
            }
        }

        //Permet de récupérer les communes triées et gère l'affichage par "page" 50 par 50.
        public async Task<IEnumerable<Commune>> GetOrderedCommune(int page, int pageSize, string sortBy, bool? ascending, string search)
        {
            if (_allCommunes.IsNullOrEmpty())
            {
                _allCommunes = await GetCommunes();
            }

            _allCommunes = SearchCommune(search);

            _allCommunes = SortCommunes(sortBy, ascending);

            return _allCommunes
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        //Gère le retour de la requête en cas de recherche 
        private List<Commune> SearchCommune(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string normalizedSearch = _stringNormalizer.NormalizeString(search);

                return _allCommunes = _allCommunes.Where(c =>
                    c.Nom != null && _stringNormalizer.NormalizeString(c.Nom).Contains(normalizedSearch) ||
                    c.Code != null && c.Code.Contains(search) ||
                    c.CodeDepartement != null && c.CodeDepartement.Contains(search) ||
                    c.CodesPostaux != null && c.CodesPostaux.Any(cp => cp.Contains(search))).ToList();
            }
            {
                return _allCommunes;
            }
        }

        //Permet de trier les communes en fonction de du critère par ordre asc et desc
        private List<Commune> SortCommunes(string sortBy, bool? ascending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return _allCommunes.ToList();
            }


            PropertyInfo property = GetPropertyValue(sortBy);

            if (property == null || !ascending.HasValue)
            {
                return _allCommunes.ToList();
            }

            return ascending.Value
                ? _allCommunes.OrderBy(c => property.GetValue(c, null)).ToList()
                : _allCommunes.OrderByDescending(c => property.GetValue(c, null)).ToList();
        }

        //Me retourne via la reflexion la valeur de la propriété de mon objet en comparant les noms sans tenir compte des MAJ etc...
        private PropertyInfo GetPropertyValue(string propertyName)
        {
            return typeof(Commune).GetProperties()
                .FirstOrDefault(property => string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
