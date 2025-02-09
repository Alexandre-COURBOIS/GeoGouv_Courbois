using GeoGouv_Courbois.Models;

namespace GeoGouv_Courbois.Services.Interfaces
{
    public interface IVilleService
    {
        Task<List<Ville>> GetVilles();

        Task<IEnumerable<Ville>> GetOrdereVille(int page, int pageSize, string sortBy, bool? ascending, string search);
        bool InsertVilleAndDepartementInDB(Departement dpt, Ville ville);

        Task<Ville> GetVilleDetails(String codeVille);

        Task<Departement> GetDepartementDetails(String codeDepartement);
    }
}
