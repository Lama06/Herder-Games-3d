using UnityEngine;
using System.Collections.Generic;

namespace HerderGames.Lehrer.Sprache
{
    public class SaetzeMoeglichkeitenMehrmals : ISaetzeMoeglichkeitenMehrmals
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

        public (string satz, float? delay) GetNextSatz()
        {
            float? delay = UseCustomDelay ? CustomDelay : null;
            if (MoeglicheSaetze.Count == 0)
            {
                return (null, delay);
            }
            var satz = MoeglicheSaetze[Random.Range(0, MoeglicheSaetze.Count)];
            return (satz, delay);
        }
    }
}
