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
            var bigger = ToFloat() > other.ToFloat() ? this : other;
            var smaller = ToFloat() > other.ToFloat() ? other : this;
            var diff = bigger.ToFloat() - smaller.ToFloat();
            return ZeitDauer.FromFloat(diff);
        }

        private float ToFloat()
        {
            return Time + (float) Wochentag * 24f + Kalenderwoche * 7f * 24f;
        }
    }
}