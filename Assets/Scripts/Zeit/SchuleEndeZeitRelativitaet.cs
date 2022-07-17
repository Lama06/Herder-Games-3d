using System.Collections.Generic;
using System.Linq;

namespace HerderGames.Zeit
{
    public class SchuleEndeZeitRelativitaet : ZeitRelativitaetBase
    {
        public override IEnumerable<float> GetZeitVerschiebungen(Wochentag tag)
        {
            var letzteStunde = StundenPlanRaster.GetTagesAblauf(tag).Last();
            return new List<float> {letzteStunde.Ende};
        }
    }
}