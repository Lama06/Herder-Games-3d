using System.Collections;
using HerderGames.AI.Sensors;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(VisionSensor))]
    public class VerbrechenErkennen : LehrerGoalBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private Saetze Reaktion;

        private VisionSensor Vision;

        protected override void Awake()
        {
            base.Awake();
            Vision = GetComponent<VisionSensor>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Vision.CanSee(Player.gameObject) && Player.VerbrechenManager.BegehtGeradeEinVerbrechen;
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SayRandomNow(Reaktion);
            Player.Chat.SendChatMessage(
                "Du wurdest von einem Lehrer erkannt und hast bei diesem Lehrer Reputationspunkte verloren." +
                "Sei vorsichtig, dass er dich nicht bei der Schulleitubg meldet!");
            Lehrer.Reputation.AddReputation(-Player.VerbrechenManager.Schwere);
            Player.VerbrechenManager.VerbrechenAbbrechen();
            yield return null;
        }
    }
}