using System;
using System.Collections.Generic;

namespace HerderGames.Lehrer.Sprache
{
    [Serializable]
    public class SaetzeMoeglichkeitenMehrmals
    {
        public bool UseCustomDelay;
        public float CustomDelay;
        public IList<string> MoeglicheSaetze;
        
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