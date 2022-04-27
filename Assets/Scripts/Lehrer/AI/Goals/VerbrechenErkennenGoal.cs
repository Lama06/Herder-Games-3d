using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VerbrechenErkennenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Player.Player Player;
        private readonly float SchwereMindestens;
        private readonly SaetzeMoeglichkeitenEinmalig Reaktion;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly VisionSensor Vision;

        private bool GehtGeradeZuTatort;

        public VerbrechenErkennenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Player.Player player,
            float schwereMindestens,
            SaetzeMoeglichkeitenEinmalig reaktion,
            SaetzeMoeglichkeitenMehrmals saetzeWeg,
            VisionSensor vision
        ) : base(lehrer)
        {
            Trigger = trigger;
            Player = player;
            SchwereMindestens = schwereMindestens;
            Reaktion = reaktion;
            SaetzeWeg = saetzeWeg;
            Vision = vision;
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
            Player.Chat.SendChatMessage("Achtung: Ein Lehrer hat gemerkt, dass du ein Verbrechen begehen wolltest. " +
                                        "Pass auf, dass er nicht zur Schulleitung geht und dich meldet. " +
                                        "Versuche deine Beziehung zum Lehrer wieder zu verbessern.");
            
            GehtGeradeZuTatort = true;
            Lehrer.Sprache.Say(Reaktion);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Reputation.AddReputation(-Player.VerbrechenManager.Schwere);
            Player.VerbrechenManager.VerbrechenAbbrechen();
            Lehrer.Agent.destination = Player.transform.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            yield return new WaitForSeconds(5f);
            GehtGeradeZuTatort = false;
        }
    }
}
