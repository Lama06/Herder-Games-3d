namespace HerderGames.Lehrer.Sprache
{
    public interface ISaetzeMoeglichkeitenMehrmals
    {
        public (string satz, float? delay) NextSatz { get; }
    }
}
