using System.Collections;
using HerderGames.AI.Sensors;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(VisionSensor))]
    public class VerbrechenErkennen : LehrerGoalBase
    {
        [SerializeField] private Player.Player Player;
        
        private VisionSensor Vision;

        protected override void Awake()
        {
            base.Awake();
            Vision = GetComponent<VisionSensor>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Vision.CanSee(Player.gameObject) && Player.GetVerbrechenManager().BegehtGeradeEinVerbrechen;
        }

        public override IEnumerator Execute()
        {
            Player.GetChat().SendChatMessage(Lehrer, "Hey? Was machst du da?");
            Player.GetChat().SendChatMessage("Du wurdest von einem Lehrer erkannt und hast bei diesem Lehrer Reputationspunkte verloren." +
                                             "Sei vorsichtig, dass er dich nicht bei der Schulleitubg meldet!");
            Lehrer.GetReputation().AddReputation(-Player.GetVerbrechenManager().Schwere);
            Player.GetVerbrechenManager().VerbrechenAbbrechen();
            yield return null;
        }
    }
}
