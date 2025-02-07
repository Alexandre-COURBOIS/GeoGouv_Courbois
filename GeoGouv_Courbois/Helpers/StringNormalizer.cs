using System.Globalization;
using System.Text;

namespace GeoGouv_Courbois.Helpers
{
    public class StringNormalizer
    {
        public string NormalizeString(string stringInput)
        {
            if (string.IsNullOrWhiteSpace(stringInput))
                return "";

            string decomposed = stringInput.Normalize(NormalizationForm.FormD);
            StringBuilder filtered = new StringBuilder();

            foreach (char c in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    filtered.Append(c);
                }
            }

            return filtered.ToString()
                           .Normalize(NormalizationForm.FormC)
                           .Replace("-", " ")
                           .ToLower();
        }
    }

}
