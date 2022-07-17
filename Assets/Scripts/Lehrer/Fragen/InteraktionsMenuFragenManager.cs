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
        
        private Lehrer Lehrer;
        
        private readonly List<FrageEintrag> Fragen = new();

        public void AddFrage(InteraktionsMenuFrage frage)
        {
            Fragen.Add(new FrageEintrag
            {
                Frage = frage,
                InteraktionsMenuId = null
            });
        }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Update()
        {
            foreach (var frage in Fragen)
            {
                if (frage.ShouldShow(Lehrer) && !frage.IsShown())
                {
                    frage.InteraktionsMenuId = Player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                    {
                        Name = frage.Frage.Text,
                        Callback = _ => frage.Frage.OnClick()
                    });
                }

                if (!frage.ShouldShow(Lehrer) && frage.IsShown())
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

            public bool ShouldShow(Lehrer lehrer)
            {
                return Frage.ShouldShow && lehrer.InSchule.GetInSchule();
            }
        }
    }
}
