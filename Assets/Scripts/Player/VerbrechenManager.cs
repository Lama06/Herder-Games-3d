using System;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class VerbrechenManager : MonoBehaviour
    {
        public bool BegehtGeradeEinVerbrechen { get; private set; }
        public float TimeRemaining { get; private set; }
        public float Schwere { get; private set; }
        private Action Callback;
        private int LastProgressMessage;

        private Player Player;

        private void Awake()
        {
            Player = GetComponent<Player>();
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

            TimeRemaining -= UnityEngine.Time.deltaTime;

            if (TimeRemaining <= 0)
            {
                Callback();
                BegehtGeradeEinVerbrechen = false;
                TimeRemaining = 0;
                Callback = null;
                Schwere = 0;
                Player.Chat.SendChatMessage("Verbrechen wurde beendet");
            }
            else if ((int) TimeRemaining < LastProgressMessage)
            {
                LastProgressMessage = (int) TimeRemaining;
                Player.Chat.SendChatMessage($"Verbrechen ist in {LastProgressMessage} Sekunden beendet");
            }
        }

        public void VerbrechenStarten(float time, float schwere, Action callback)
        {
            BegehtGeradeEinVerbrechen = true;
            TimeRemaining = time;
            Schwere = schwere;
            Callback = callback;
            LastProgressMessage = ((int) time) + 1;
        }

        public void VerbrechenAbbrechen()
        {
            BegehtGeradeEinVerbrechen = false;
            TimeRemaining = 0;
            Schwere = 0;
            Callback = null;
            LastProgressMessage = 0;
        }
    }
}