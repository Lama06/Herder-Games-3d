using System;

namespace HerderGames.Lehrer.Fragen
{
    public abstract class InteraktionsMenuFrage
    {
        protected Lehrer Lehrer { get; private set; }

        public void InitLehrer(Lehrer lehrer)
        {
            if (Lehrer != null)
            {
                throw new Exception($"{nameof(InitLehrer)} called twice");
            }

            Lehrer = lehrer;
        }
        
        public abstract bool ShouldShow { get; }

        public abstract string Text { get; }

        public abstract void OnClick();
    }
}
