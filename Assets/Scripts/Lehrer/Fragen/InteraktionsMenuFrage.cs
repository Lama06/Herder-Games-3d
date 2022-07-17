namespace HerderGames.Lehrer.Fragen
{
    public abstract class InteraktionsMenuFrage
    {
        public abstract bool ShouldShow { get; }

        public abstract string Text { get; }

        public abstract void OnClick();
    }
}
