using System;
using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class StundeZeitRelativitaet : ZeitRelativitaetBase
    {
        private readonly StundenType Stunde;
        private readonly int? Index;
        private readonly AnfangOderEnde AnfangEnde;

        public StundeZeitRelativitaet(StundenType stunde, int? index, AnfangOderEnde anfangEnde)
        {
            Stunde = stunde;
            Index = index;
            AnfangEnde = anfangEnde;
        }

        public StundeZeitRelativitaet(StundenType stunde, AnfangOderEnde anfangEnde) : this(stunde, null, anfangEnde)
        {
        }

        public override IList<float> Resolve(Wochentag tag)
        {
            var result = new List<float>();

            var currentIndex = 0;
            foreach (var eintrag in StundenPlanRaster.GetTagesAblauf(tag))
            {
                if (eintrag.Stunde != Stunde)
                {
                    continue;
                }

                if (Index != null && Index != currentIndex)
                {
                    currentIndex++;
                    continue;
                }
                
                currentIndex++;

                result.Add(AnfangEnde.ResolveStunde(eintrag));
            }

            return result;
        }
    }
}
