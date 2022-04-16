namespace HerderGames.Time
{
    [System.Serializable]
    public class Zeitspanne
    {
        public Zeitpunkt Start;
        public Zeitpunkt Ende;

        public ZeitDauer Laenge()
        {
            return Start.Diff(Ende);
        }
    }
}