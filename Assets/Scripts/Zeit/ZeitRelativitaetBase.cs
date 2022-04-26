using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public abstract class ZeitRelativitaetBase
    {
        public abstract IList<float> Resolve(Wochentag tag);
    }
}