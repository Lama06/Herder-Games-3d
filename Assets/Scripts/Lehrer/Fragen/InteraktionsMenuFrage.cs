namespace HerderGames.Lehrer.Fragen
{
    public abstract class InteraktionsMenuFrage
    {
        public abstract bool ShouldShow();

        public abstract string GetText();

        public abstract void OnClick(int id);
    }
}
