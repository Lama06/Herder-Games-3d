using System.Collections.Generic;
using HerderGames.Time.Stundenplan;

namespace HerderGames.Time.Data
{
    [System.Serializable]
    public class Zeitspanne
    {
        public WochentagEintrag[] Wochentage;
        
        public IList<(Wochentag, float, float)> Resolve()
        {
            var result = new List<(Wochentag, float, float)>();
            
            foreach (var wochentagEintrag in Wochentage)
            {
                result.AddRange(wochentagEintrag.Resolve());
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
        
        [System.Serializable]
        public class WochentagEintrag
        {
            public WochentagAuswahlArt AuswahlArt;
            public Wochentag[] Manuell;
            public ZeitspanneEintrag[] Zeitspannen;
            
            public IList<(Wochentag, float, float)> Resolve()
            {
                var result = new List<(Wochentag, float, float)>();
                
                foreach (var wochentag in AuswahlArt.ResolveWochentage(Manuell))
                {
                    foreach (var zeitspanneEintrag in Zeitspannen)
                    {
                        foreach (var (start, end) in zeitspanneEintrag.Resolve(wochentag))
                        {
                            result.Add((wochentag, start, end));
                        }
                    }
                }

                return result;
            }
            
            [System.Serializable]
            public class ZeitspanneEintrag
            {
                public Zeitpunkt Anfang;
                public Zeitpunkt Ende;

                public IList<(float, float)> Resolve(Wochentag tag)
                {
                    var result = new List<(float, float)>();
                    
                    var zeitenAnfang = Anfang.Resolve(tag);
                    var zeitenEnde = Ende.Resolve(tag);

                    if (zeitenAnfang.Count != zeitenEnde.Count)
                    {
                        return new List<(float, float)>();
                    }
                
                    for (var i = 0; i < zeitenAnfang.Count; i++)
                    {
                        var zeitAnfang = zeitenAnfang[i];
                        var zeitEnde = zeitenEnde[i];
                    
                        result.Add((zeitAnfang, zeitEnde));
                    }

                    return result;
                }

                [System.Serializable]
                public class Zeitpunkt
                {
                    public ZeitRelativitaet RelativZu;
                    public int RelativZuN;
                    public float Zeit;

                    public IList<float> Resolve(Wochentag tag)
                    {
                        return RelativZu.ResolveToAbsoluteTime(tag, Zeit, RelativZuN);
                    }
                }
            }
        }
    }
}
