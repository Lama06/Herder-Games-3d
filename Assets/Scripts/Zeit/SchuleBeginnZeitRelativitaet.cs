using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class SchuleBeginnZeitRelativitaet : ZeitRelativitaetBase
    {
        public override IEnumerable<float> GetZeitVerschiebungen(Wochentag tag)
        {
            return new List<float> {StundenPlanRaster.SchuleBeginn};
        }
    }
}