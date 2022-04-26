using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class SchuleBeginnZeitRelativitaet : ZeitRelativitaetBase
    {
        public override IList<float> Resolve(Wochentag tag)
        {
            return new List<float> {StundenPlanRaster.SchuleBeginn};
        }
    }
}