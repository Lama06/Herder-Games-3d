using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public class WoechentlicheZeitspannen
    {
        private readonly IList<Eintrag> Eintraege;

        public WoechentlicheZeitspannen(params Eintrag[] eintraege)
        {
            Eintraege = eintraege;
        }

        public IList<(Wochentag, float, float)> Resolve()
        {
            var result = new List<(Wochentag, float, float)>();

            foreach (var eintrag in Eintraege)
            {
                result.AddRange(eintrag.Resolve());
            }
            
            return result;
        }
        
        public bool IsInside(Wochentag tag, float time)
        {
            foreach (var (wochentag, start, end) in Resolve())
            {
                if (wochentag != tag)
                {
                    continue;
                }

                if (start <= time && end >= time)
                {
                    return true;
                }
            }

            return false;
        }

        public class Eintrag
        {
            private readonly WochentagAuswahlBase Tage;
            private readonly IList<Zeitspanne> Zeitspannen;

            public Eintrag(WochentagAuswahlBase tage, params Zeitspanne[] zeitspannen)
            {
                Tage = tage;
                Zeitspannen = zeitspannen;
            }

            public IList<(Wochentag, float, float)> Resolve()
            {
                var result = new List<(Wochentag, float, float)>();
                
                foreach (var wochentag in Tage.ResolveWochentage())
                {
                    foreach (var zeitspanne in Zeitspannen)
                    {
                        foreach (var (anfang, ende) in zeitspanne.Resolve(wochentag))
                        {
                            result.Add((wochentag, anfang, ende));
                        }
                    }
                }

                return result;
            }
        }

        public class Zeitspanne
        {
            private readonly Zeitpunkt Anfang;
            private readonly Zeitpunkt Ende;

            public Zeitspanne(Zeitpunkt anfang, Zeitpunkt ende)
            {
                Anfang = anfang;
                Ende = ende;
            }

            public IList<(float, float)> Resolve(Wochentag tag)
            {
                var zeitenAnfang = Anfang.Resolve(tag);
                var zeitenEnde = Ende.Resolve(tag);

                if (zeitenAnfang.Count != zeitenEnde.Count)
                {
                    return new List<(float, float)>();
                }

                var result = new List<(float, float)>();

                for (var i = 0; i < zeitenAnfang.Count; i++)
                {
                    var zeitAnfang = zeitenAnfang[i];
                    var zeitEnde = zeitenEnde[i];

                    result.Add((zeitAnfang, zeitEnde));
                }

                return result;
            }
        }

        public class Zeitpunkt
        {
            private readonly ZeitRelativitaetBase RelativZu;
            private readonly float Offset;

            public Zeitpunkt(ZeitRelativitaetBase relativZu, float offset)
            {
                RelativZu = relativZu;
                Offset = offset;
            }

            public IList<float> Resolve(Wochentag tag)
            {
                var result = new List<float>();

                foreach (var baseTime in RelativZu.Resolve(tag))
                {
                    result.Add(baseTime + Offset);
                }

                return result;
            }
        }
    }
}