using System;
using UnityEngine;

namespace HerderGames.Time
{
    [Serializable]
    public class ZeitDauer
    {
        [SerializeField] private int Wochen;
        [SerializeField] private int Tage;
        [SerializeField] private float Time;

        public int GetAnzahlTage()
        {
            return Wochen * 7 + Tage;
        }

        public float ToSekunden()
        {
            return ToStunden() / (60 * 60);
        }
        
        public float ToStunden()
        {
            return Wochen * 24f * 7f + Tage * 24f + Time;
        }
        
        public static ZeitDauer FromStunden(float total)
        {
            var wochen = (int) (total / (24f * 7f));
            total -= wochen * 24f * 7f;
            var tage = (int) (total / 24f);
            total -= tage * 24f;
            var stunden = total;

            return new ZeitDauer
            {
                Wochen = wochen,
                Tage = tage,
                Time = stunden
            };
        }

        public bool IsLongerThan(ZeitDauer other)
        {
            return ToStunden() > other.ToStunden();
        }
    }
}