using System.Collections.Generic;

namespace HerderGames.Lehrer.Sprache
{
    public class SaetzeMoeglichkeitenMehrmals
    {
        public readonly bool UseCustomDelay;
        public readonly float CustomDelay;
        public readonly IList<string> MoeglicheSaetze;
        
        public SaetzeMoeglichkeitenMehrmals(params string[] saetze)
        {
            MoeglicheSaetze = saetze;
        }

        public SaetzeMoeglichkeitenMehrmals(float delay, params string[] saetze) : this(saetze)
        {
            UseCustomDelay = true;
            CustomDelay = delay;
        }
    }
}