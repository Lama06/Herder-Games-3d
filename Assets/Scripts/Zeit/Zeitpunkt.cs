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
        
        public Dauer Diff(Zeitpunkt other)
        {
            var bigger = StundenSeitBeginn > other.StundenSeitBeginn ? this : other;
            var smaller = StundenSeitBeginn > other.StundenSeitBeginn ? other : this;
            var diff = bigger.StundenSeitBeginn - smaller.StundenSeitBeginn;
            return Dauer.FromStunden(diff);
        }

        private float StundenSeitBeginn => Zeit + (float) Wochentag * 24f + Kalenderwoche * 7f * 24f;
    }
}