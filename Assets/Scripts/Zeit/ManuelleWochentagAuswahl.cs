using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class ManuelleWochentagAuswahl : WochentagAuswahlBase
    {
        private readonly ISet<Wochentag> Wochentage;

        public ManuelleWochentagAuswahl(params Wochentag[] wochentage)
        {
            Wochentage = new HashSet<Wochentag>(wochentage);
        }
        
        public override ISet<Wochentag> ResolveWochentage()
        {
            return Wochentage;
        }
    }
}