using System;
using System.Collections.Generic;
using System.Linq;

namespace HerderGames.Zeit
{
    public enum WochentagEigenschaft
    {
        Kurztag,
        LangtagLang,
        LangtagNormal,
        Wochenende,
        Schultag
    }

    public static class WochentagEigenschaftExtensions
    {
        public static ISet<Wochentag> GetWochentage(this WochentagEigenschaft eigenschaft)
        {
            var result = new HashSet<Wochentag>();

            foreach (var wochentag in Enum.GetValues(typeof(Wochentag)).Cast<Wochentag>())
            {
                if (wochentag.GetEigenschaften().Contains(eigenschaft))
                {
                    result.Add(wochentag);
                }
            }
            
            return result;
        }
    }
}