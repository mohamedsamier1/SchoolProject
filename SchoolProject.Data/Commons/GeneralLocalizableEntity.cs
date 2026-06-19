using System.Globalization;

namespace SchoolProject.Data.Commons
{
    public class GeneralLocalizableEntity
    {
        public string GenerallLocalizedName(string textAr, string textEn)
        {
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";
            if (culture)
            {
                return textAr;
            }
            return textEn;
        }
    }
}
