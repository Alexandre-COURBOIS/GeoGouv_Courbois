using System.Text.Json.Serialization;

namespace GeoGouv_Courbois.Models
{
    public class Commune
    {
        [JsonPropertyName("nom")]
        public string? Nom { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("codeDepartement")]
        public string? CodeDepartement { get; set; }

        [JsonPropertyName("siren")]
        public string? Siren { get; set; }

        [JsonPropertyName("codeEpci")]
        public string? CodeEpci { get; set; }

        [JsonPropertyName("codeRegion")]
        public string? CodeRegion { get; set; }

        [JsonPropertyName("codesPostaux")]
        public List<string>? CodesPostaux { get; set; }

        [JsonPropertyName("population")]
        public int? Population { get; set; }

    }
}
