using System.Collections.Generic;
using System.Linq;

namespace HerderGames.Lehrer.Sprache
{
    public class SaetzeMoeglichkeitenEinmalig
    {
        public readonly List<string> MoeglicheSaetze;

        public SaetzeMoeglichkeitenEinmalig(params string[] saetze)
        {
            MoeglicheSaetze = saetze.ToList();
        }
    }
}
