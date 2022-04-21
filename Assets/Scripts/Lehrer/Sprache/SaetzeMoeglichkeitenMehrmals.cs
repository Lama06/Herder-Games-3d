using System;

namespace HerderGames.Lehrer.Sprache
{
    [Serializable]
    public class SaetzeMoeglichkeitenMehrmals
    {
        public bool UseCustomDelay;
        public float CustomDelay;
        public SatzMoeglichkeit[] MoeglicheSaetze;
        public string[] SharedIds;
    }
}