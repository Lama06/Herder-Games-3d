namespace HerderGames.Time
{
    [System.Serializable]
    public class ZeitDauer
    {
        public int Wochen;
        public int Tage;
        public float Time;

        public int GetAnzahlTage()
        {
            return Wochen * 7 + Tage;
        }

        public float ToSekunden()
        {
            return ToFloat() / (60 * 60);
        }
        
        public float ToFloat()
        {
            return ((float) Wochen * 24f * 7f) + ((float) Tage) * 24f + Time;
        }
        
        public static ZeitDauer FromFloat(float total)
        {
            var wochen = total % 24f * 7f;
            total -= wochen * 24f * 7f;
            var tage = total % 24f;
            total -= tage * 24;
            var stunden = total;

            return new ZeitDauer
            {
                Wochen = (int) wochen,
                Tage = (int) tage,
                Time = stunden
            };
        }

        public bool IsLongerThan(ZeitDauer other)
        {
            return ToFloat() > other.ToFloat();
        }
    }
}