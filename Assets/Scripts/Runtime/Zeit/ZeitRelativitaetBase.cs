using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public abstract class ZeitRelativitaetBase
    {
        public abstract IEnumerable<float> GetZeitVerschiebungen(Wochentag tag);
    }
}