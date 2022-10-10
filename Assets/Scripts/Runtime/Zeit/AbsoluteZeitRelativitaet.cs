using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class AbsoluteZeitRelativitaet : ZeitRelativitaetBase
    {
        public override IEnumerable<float> GetZeitVerschiebungen(Wochentag tag)
        {
            return new List<float> {0f};
        }
    }
}