using UnityEngine;
using System.Collections.Generic;

namespace HerderGames.Lehrer.Sprache
{
    public class SaetzeMoeglichkeitenMehrmals : ISaetzeMoeglichkeitenMehrmals
    {
        public readonly bool UseCustomDelay;
        public readonly float CustomDelay;
        public readonly IList<string> MoeglicheSaetze;

        private List<string> RemainingSaetze;

        public SaetzeMoeglichkeitenMehrmals(params string[] saetze)
        {
            MoeglicheSaetze = saetze;

            RemainingSaetze = new List<string>(MoeglicheSaetze);
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

            if (RemainingSaetze.Count == 0)
            {
                RemainingSaetze = new List<string>(MoeglicheSaetze);
            }
            
            var satz = RemainingSaetze[Random.Range(0, RemainingSaetze.Count)];
            RemainingSaetze.Remove(satz);
            return (satz, delay);
        }
    }
}
