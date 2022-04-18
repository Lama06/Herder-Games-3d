namespace HerderGames.Time
{
    [System.Serializable]
    public class Zeitpunkt
    {
        public int Kalenderwoche;
        public Wochentag Wochentag;
        public float Time;

        public bool IstAmSelbenTagWie(Zeitpunkt other)
        {
            return other.Kalenderwoche == Kalenderwoche && other.Wochentag == Wochentag;
        }
        
        public ZeitDauer Diff(Zeitpunkt other)
        {
            var bigger = ToStunden() > other.ToStunden() ? this : other;
            var smaller = ToStunden() > other.ToStunden() ? other : this;
            var diff = bigger.ToStunden() - smaller.ToStunden();
            return ZeitDauer.FromStunden(diff);
        }

        public float ToStunden()
        {
            return Time + (float) Wochentag * 24f + (Kalenderwoche - 1) * 7f * 24f;
        }
    }
}