using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HerderGames.Lehrer.Sprache
{
    public class SaetzeMoeglichkeitenEinmalig : ISaetzeMoeglichkeitenEinmalig
    {
        public readonly List<string> MoeglicheSaetze;

        public SaetzeMoeglichkeitenEinmalig(params string[] saetze)
        {
            MoeglicheSaetze = saetze.ToList();
        }
        
        public string GetSatz()
        {
            if (MoeglicheSaetze.Count == 0)
            {
                return null;
            }
            return MoeglicheSaetze[Random.Range(0, MoeglicheSaetze.Count)];
        }
    }
}
