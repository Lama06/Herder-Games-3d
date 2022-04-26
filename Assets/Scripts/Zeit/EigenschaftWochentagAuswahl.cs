using System;
using System.Collections.Generic;
using System.Linq;

namespace HerderGames.Zeit
{
    public class EigenschaftWochentagAuswahl : WochentagAuswahlBase
    {
        private readonly WochentagEigenschaft Eigenschaft;

        public EigenschaftWochentagAuswahl(WochentagEigenschaft eigenschaft)
        {
            Eigenschaft = eigenschaft;
        }

        public override ISet<Wochentag> ResolveWochentage()
        {
            var result = new HashSet<Wochentag>();

            foreach (var wochentag in Enum.GetValues(typeof(Wochentag)).Cast<Wochentag>())
            {
                if (wochentag.GetEigenschaften().Contains(Eigenschaft))
                {
                    result.Add(wochentag);
                }
            }

            return result;
        }
    }
}