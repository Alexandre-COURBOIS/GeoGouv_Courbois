using GeoGouv_Courbois.Models;

namespace GeoGouv_Courbois.Services.Interfaces
{
    public interface ICommuneService
    {
        Task<List<Commune>> GetCommunes();

        Task<IEnumerable<Commune>> GetOrderedCommune(int page, int pageSize, string sortBy, bool? ascending, string search);
    }
}
