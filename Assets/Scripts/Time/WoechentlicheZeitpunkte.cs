using System.Collections.Generic;

namespace HerderGames.Time
{
    [System.Serializable]
    public class WoechentlicheZeitpunkte
    {
        public WochentagEintrag[] Wochentage;

        public IList<(Wochentag, float)> Resolve()
        {
            var result = new List<(Wochentag, float)>();

            foreach (var wochentagEintrag in Wochentage)
            {
                result.AddRange(wochentagEintrag.Resolve());
            }

            return result;
        }

        [System.Serializable]
        public class WochentagEintrag
        {
            public WochentagAuswahlArt AuswahlArt;
            public Wochentag[] Manuell;
            public ZeitpunktEintrag[] Zeitpunkte;

            public IList<(Wochentag, float)> Resolve()
            {
                var result = new List<(Wochentag, float)>();
                
                foreach (var wochentag in AuswahlArt.ResolveWochentage(Manuell))
                {
                    foreach (var zeitpunktEintrag in Zeitpunkte)
                    {
                        foreach (var zeitpunkt in zeitpunktEintrag.Resolve(wochentag))
                        {
                            result.Add((wochentag, zeitpunkt));
                        }
                    }
                }

                return result;
            }

            [System.Serializable]
            public class ZeitpunktEintrag
            {
                public ZeitRelativitaet RelativZu;
                public int RelativZuN;
                public float Zeit;

                public IList<float> Resolve(Wochentag wochentag)
                {
                    return RelativZu.ResolveToAbsoluteTime(wochentag, Zeit, RelativZuN);
                }
            }
        }
    }
}