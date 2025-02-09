namespace GeoGouv_Courbois.Services.Interfaces
{
    public interface ISqlService {

        public bool InsertVilleAndDepartement(string codePostalDepartement, string nomDepartement,string commune, string nomCommuneMinuscule,string codePostal, 
                                            string codeInsee);

        List<Dictionary<string, object>> GetAllDepartementsWithVilles();
    }

}
