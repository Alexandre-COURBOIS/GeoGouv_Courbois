using System.Text.Json.Serialization;

namespace GeoGouv_Courbois.Models
{
    public class Departement
    {
        [JsonPropertyName("nom")]
        public string? Nom { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("codeRegion")]
        public string? CodeRegion { get; set; }
    }
}
