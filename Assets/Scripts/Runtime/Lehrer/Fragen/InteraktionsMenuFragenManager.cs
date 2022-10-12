using System.Collections.Generic;
using HerderGames.Player;

namespace HerderGames.Lehrer.Fragen
{
    public class InteraktionsMenuFragenManager
    {
        private readonly Lehrer Lehrer;
        private readonly List<FrageEintrag> Fragen = new();
        private bool _Enabled;
        
        public InteraktionsMenuFragenManager(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }
        
        public bool Enabled
        {
            get => _Enabled;
            set
            {
                if (!value)
                {
                    foreach (var frage in Fragen)
                    {
                        frage.Shown = false;
                    }
                }
                
                _Enabled = value;
            }
        }
        
        public void AddFrage(InteraktionsMenuFrage frage)
        {
            frage.InitLehrer(Lehrer);
            
            Fragen.Add(new FrageEintrag
            {
                Player = Lehrer.Player,
                Frage = frage,
                InteraktionsMenuId = null
            });
        }

        public void Update()
        {
            foreach (var frage in Fragen)
            {
                frage.Update();
            }
        }

        private class FrageEintrag
        {
            public Player.Player Player;
            public InteraktionsMenuFrage Frage;
            public int? InteraktionsMenuId;

            public void Update()
            {
                Shown = ShouldShow;
            }

            public bool Shown
            {
                get => InteraktionsMenuId != null;

                set
                {
                    if (value == Shown)
                    {
                        return;
                    }

                    if (value)
                    {
                        InteraktionsMenuId = Player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                        {
                            Name = Frage.Text,
                            Callback = _ => Frage.OnClick()
                        });
                    }
                    else
                    {
                        Player.InteraktionsMenu.RemoveEintrag((int) InteraktionsMenuId);
                        InteraktionsMenuId = null;
                    }
                }
            }

            private bool ShouldShow => Frage.ShouldShow;
        }
    }
}