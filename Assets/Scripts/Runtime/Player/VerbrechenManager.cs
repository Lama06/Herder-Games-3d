using System;
using System.Collections;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class VerbrechenManager : MonoBehaviour
    {
        public bool BegehtGeradeEinVerbrechen { get; private set; }
        public float Schwere { get; private set; }
        private int TimeRemaining;
        private Action Callback;

        private Player Player;

        private void Awake()
        {
            Player = GetComponent<Player>();
        }

        private void Start()
        {
            StartCoroutine(TickTime());
            StartCoroutine(ListenForCancel());
        }

        private IEnumerator TickTime()
        {
            while (true)
            {
                while (BegehtGeradeEinVerbrechen)
                {
                    if (TimeRemaining > 0)
                    {
                        Player.Chat.SendChatMessage($"Verbrechen in {TimeRemaining} Sekunden beendet");   
                    }
                    else
                    {
                        Player.Chat.SendChatMessage("Verbrechen wurde beendet");
                        Callback();
                        BegehtGeradeEinVerbrechen = false;
                        TimeRemaining = 0;
                        Callback = null;
                        Schwere = 0;
                    }
                    yield return new WaitForSeconds(1);
                    TimeRemaining--;
                }

                yield return null;
            }
        }

        private IEnumerator ListenForCancel()
        {
            while (true)
            {
                if (BegehtGeradeEinVerbrechen && Input.GetKeyDown(KeyCode.C))
                {
                    VerbrechenAbbrechen();
                    Player.Chat.SendChatMessage("Verbrechen abgebrochen");
                }

                yield return null;
            }
        }

        public void VerbrechenStarten(int time, float schwere, Action callback)
        {
            if (BegehtGeradeEinVerbrechen)
            {
                return;
            }

            BegehtGeradeEinVerbrechen = true;
            TimeRemaining = time;
            Schwere = schwere;
            Callback = callback;
        }

        public void VerbrechenAbbrechen()
        {
            if (!BegehtGeradeEinVerbrechen)
            {
                return;
            }
            
            BegehtGeradeEinVerbrechen = false;
            TimeRemaining = 0;
            Schwere = 0;
            Callback = null;
        }
    }
}