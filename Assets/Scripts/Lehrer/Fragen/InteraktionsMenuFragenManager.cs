using System;
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
                Player = Player,
                Frage = frage,
                InteraktionsMenuId = null
            });
        }

        private void Update()
        {
            foreach (var frage in Fragen)
            {
                frage.Update();
            }
        }

        private void OnDisable()
        {
            foreach (var frage in Fragen)
            {
                frage.Shown = false;
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
