using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(Krankheit))]
    public class BeiKranktheitZuhauseBleiben : GoalBase
    {
        private Krankheit Krankheit;

        private void Awake()
        {
            Krankheit = GetComponent<Krankheit>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Krankheit.IsKrank();
        }

        public override void OnStarted()
        {
            if (Krankheit.GrundDerErkrankung.Status == VergiftungsStatus.VergiftetNichtBemerkt)
            {
                Krankheit.GrundDerErkrankung.Status = VergiftungsStatus.VergiftetBemerkt;
            }
        }
    }
}