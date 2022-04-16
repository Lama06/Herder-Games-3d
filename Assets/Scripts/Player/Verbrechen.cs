using System;
using UnityEngine;

namespace HerderGames.Player
{
    public class Verbrechen : MonoBehaviour
    {
        public bool BegehtGeradeEinVerbrechen { get; private set; }
        public float TimeRemaining { get; private set; }
        private Action Callback;
        public float Schwere { get; private set; }

        private void Update()
        {
            if (!BegehtGeradeEinVerbrechen)
            {
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
            }
        }

        public void VerbrechenStarten(float time, float schwere, Action callback)
        {
            BegehtGeradeEinVerbrechen = true;
            TimeRemaining = time;
            Callback = callback;
        }

        public void VerbrechenAbbrechen()
        {
            BegehtGeradeEinVerbrechen = false;
            TimeRemaining = 0;
            Callback = null;
        }
    }
}