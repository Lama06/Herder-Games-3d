using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class ManuelleWochentagAuswahl : WochentagAuswahlBase
    {
        public ManuelleWochentagAuswahl(params Wochentag[] wochentage)
        {
            Wochentage = new HashSet<Wochentag>(wochentage);
        }

        public override ISet<Wochentag> Wochentage { get; }
    }
}