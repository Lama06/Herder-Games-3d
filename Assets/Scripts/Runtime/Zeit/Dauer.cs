using System;

namespace HerderGames.Zeit
{
    public class Dauer
    {
        public int Wochen { get; private set; }
        public int Tage { get; private set; }
        public float Stunden { get; private set; }
        
        public float Laenge => Wochen * 24f * 7f + Tage * 24f + Stunden;

        private Dauer() { }
        
        public static Dauer FromStunden(float total)
        {
            var wochen = (int) (total / (24f * 7f));
            total -= wochen * 24f * 7f;
            var tage = (int) (total / 24f);
            total -= tage * 24f;
            var stunden = total;

            return new Dauer
            {
                Wochen = wochen,
                Tage = tage,
                Stunden = stunden
            };
        }

        public static bool operator >(Dauer a, Dauer b)
        {
            return a.Laenge > b.Laenge;
        }

        public static bool operator <(Dauer a, Dauer b)
        {
            return a.Laenge < b.Laenge;
        }
    }
}