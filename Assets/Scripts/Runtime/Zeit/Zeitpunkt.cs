namespace HerderGames.Zeit
{
    public class Zeitpunkt
    {
        public int Kalenderwoche;
        public Wochentag Wochentag;
        public float Zeit;

        public bool IstAmSelbenTagWie(Zeitpunkt other)
        {
            return other.Kalenderwoche == Kalenderwoche && other.Wochentag == Wochentag;
        }
        
        public static Dauer operator -(Zeitpunkt a, Zeitpunkt b)
        {
            return Dauer.FromStunden(a.StundenSeitNull - b.StundenSeitNull);
        }

        private float StundenSeitNull => Zeit + (float) Wochentag * 24f + Kalenderwoche * 7f * 24f;
    }
}