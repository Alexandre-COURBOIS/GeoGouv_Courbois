using System.Globalization;
using System.Text;

namespace GeoGouv_Courbois.Helpers
{
    public class StringNormalizer
    {
        //Permet de retirer les tirets et espaces dans une string et la passé en minuscule
        public string NormalizeString(string stringInput)
        {
            //Si ma string est vide je retourne un ensemble vide
            if (string.IsNullOrWhiteSpace(stringInput))
            {
                return "";
            }
                
            //Je décompose ma string de manière à pouvoir itérer sur chaques caractères permet de séparer les élèments avec et sans accents
            string decomposed = stringInput.Normalize(NormalizationForm.FormD);
            //Je déclare une chaine de caractère vide
            StringBuilder filtered = new StringBuilder();

            //Itération sur chaques caractères
            foreach (char c in decomposed)
            {   
                //Si mon caractère en cours de traitement est un élement avec un accent il ajouté à ma nouvelle chaine de caractère mais sans l'accent
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    filtered.Append(c);
                }
            }

            //Je retourne ma chaine de caractère sans accent sans espace ou tiret et en minuscule en format d'afficahge standard
            return filtered.ToString()
                           .Normalize(NormalizationForm.FormC)
                           .Replace("-", " ")
                           .ToLower();
        }
    }

}
