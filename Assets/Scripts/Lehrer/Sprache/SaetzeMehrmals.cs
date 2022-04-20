namespace HerderGames.Lehrer.Sprache
{
    [System.Serializable]
    public class SaetzeMehrmals
    {
        public bool UseDefaultDelay = true;
        public float CustomDelay;
        public Satz[] MoeglicheSaetze;
        public string[] SharedIds;
    }
}