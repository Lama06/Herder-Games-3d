using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public abstract class WochentagAuswahlBase
    {
        public abstract ISet<Wochentag> ResolveWochentage();
    }
}