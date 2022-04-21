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
        }

        private IEnumerator TickTime()
        {
            while (true)
            {
                while (BegehtGeradeEinVerbrechen)
                {
                    yield return new WaitForSeconds(1);
                    if (!BegehtGeradeEinVerbrechen)
                    {
                        break;
                    }
                    
                    TimeRemaining--;
                    Player.Chat.SendChatMessage($"Verbrechen in {TimeRemaining} Sekunden beendet");
                }

                yield return null;
            }
        }

        private void Update()
        {
            if (!BegehtGeradeEinVerbrechen)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                VerbrechenAbbrechen();
                Player.Chat.SendChatMessage("Verbrechen abgebrochen");
                return;
            }

            if (TimeRemaining <= 0)
            {
                Player.Chat.SendChatMessage("Verbrechen wurde beendet");
                Callback();
                BegehtGeradeEinVerbrechen = false;
                TimeRemaining = 0;
                Callback = null;
                Schwere = 0;
            }
        }

        public void VerbrechenStarten(int time, float schwere, Action callback)
        {
            BegehtGeradeEinVerbrechen = true;
            TimeRemaining = time;
            Schwere = schwere;
            Callback = callback;
        }

        public void VerbrechenAbbrechen()
        {
            BegehtGeradeEinVerbrechen = false;
            TimeRemaining = 0;
            Schwere = 0;
            Callback = null;
        }
    }
}