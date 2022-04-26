using System;
using System.Collections.Generic;
using System.Linq;

namespace HerderGames.Lehrer.Sprache
{
    [Serializable]
    public class SaetzeMoeglichkeitenEinmalig
    {
        public List<string> MoeglicheSaetze;

        public SaetzeMoeglichkeitenEinmalig(params string[] saetze)
        {
            MoeglicheSaetze = saetze.ToList();
        }
    }
}