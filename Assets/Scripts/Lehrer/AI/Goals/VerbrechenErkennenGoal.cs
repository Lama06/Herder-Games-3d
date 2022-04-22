using System.Collections;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    [RequireComponent(typeof(VisionSensor))]
    public class VerbrechenErkennenGoal : GoalBase
    {
        [SerializeField] private Trigger.Trigger Trigger;
        [SerializeField] private Player.Player Player;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig Reaktion;

        private VisionSensor Vision;

        protected override void Awake()
        {
            base.Awake();
            Vision = GetComponent<VisionSensor>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve() && Vision.CanSee(Player.gameObject) &&
                   Player.VerbrechenManager.BegehtGeradeEinVerbrechen;
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.Say(Reaktion);
            Player.Chat.SendChatMessage("Achtung: Ein Lehrer hat gemerkt, dass du ein Verbrechen begehen wolltest. " +
                                        "Pass auf, dass er nicht zur Schulleitung geht und dich meldet. " +
                                        "Versuche deine Beziehung zum Lehrer wieder zu verbessern.");
            Lehrer.Reputation.AddReputation(-Player.VerbrechenManager.Schwere);
            Player.VerbrechenManager.VerbrechenAbbrechen();
            yield break;
        }
    }
}