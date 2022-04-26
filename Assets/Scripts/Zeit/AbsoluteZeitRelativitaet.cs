using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class AbsoluteZeitRelativitaet : ZeitRelativitaetBase
    {
        public override IList<float> Resolve(Wochentag tag)
        {
            return new List<float> {0f};
        }
    }
}