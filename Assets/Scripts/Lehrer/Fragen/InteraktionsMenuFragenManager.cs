using System.Collections.Generic;
using HerderGames.Player;
using UnityEngine;

namespace HerderGames.Lehrer.Fragen
{
    [RequireComponent(typeof(Lehrer))]
    public class InteraktionsMenuFragenManager : MonoBehaviour
    {
        [SerializeField] private Player.Player Player;
        
        private readonly List<FrageEintrag> Fragen = new();

        public void AddFrage(InteraktionsMenuFrage frage)
        {
            Fragen.Add(new FrageEintrag
            {
                Frage = frage,
                InteraktionsMenuId = null
            });
        }

        private void Update()
        {
            foreach (var frage in Fragen)
            {
                if (frage.Frage.ShouldShow() && !frage.IsShown())
                {
                    frage.InteraktionsMenuId = Player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                    {
                        Name = frage.Frage.GetText(),
                        Callback = frage.Frage.OnClick
                    });
                }

                if (!frage.Frage.ShouldShow() && frage.IsShown())
                {
                    Player.InteraktionsMenu.RemoveEintrag((int) frage.InteraktionsMenuId);
                    frage.InteraktionsMenuId = null;
                }

            }
        }

        private class FrageEintrag
        {
            public InteraktionsMenuFrage Frage;
            public int? InteraktionsMenuId;

            public bool IsShown()
            {
                return InteraktionsMenuId != null;
            }
        }
    }
}
