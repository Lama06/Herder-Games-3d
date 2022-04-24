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
        [SerializeField] private float SchwereMindestens;
        [SerializeField] private SaetzeMoeglichkeitenEinmalig Reaktion;

        private VisionSensor Vision;

        private bool GehtGeradeZuTatort;

        protected override void Awake()
        {
            base.Awake();
            Vision = GetComponent<VisionSensor>();
        }

        private bool SiehtVerbrechen()
        {
            return Vision.CanSee(Player.gameObject) &&
                   Player.VerbrechenManager.BegehtGeradeEinVerbrechen &&
                   Player.VerbrechenManager.Schwere >= SchwereMindestens;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve() && (SiehtVerbrechen() || GehtGeradeZuTatort);
        }

        public override void OnGoalEnd(GoalEndReason reason)
        {
            GehtGeradeZuTatort = false;
        }

        public override IEnumerator Execute()
        {
            GehtGeradeZuTatort = true;
            Lehrer.Sprache.Say(Reaktion);
            Player.Chat.SendChatMessage("Achtung: Ein Lehrer hat gemerkt, dass du ein Verbrechen begehen wolltest. " +
                                        "Pass auf, dass er nicht zur Schulleitung geht und dich meldet. " +
                                        "Versuche deine Beziehung zum Lehrer wieder zu verbessern.");
            Lehrer.Reputation.AddReputation(-Player.VerbrechenManager.Schwere);
            Player.VerbrechenManager.VerbrechenAbbrechen();
            Lehrer.Agent.destination = Player.transform.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            yield return new WaitForSeconds(5f);
            GehtGeradeZuTatort = false;
        }
    }
}