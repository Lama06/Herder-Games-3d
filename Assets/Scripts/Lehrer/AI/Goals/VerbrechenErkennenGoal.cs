using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VerbrechenErkennenGoal : GoalBase
    {
        private const string WarningMsg = "Achtung: Ein Lehrer hat gemerkt, dass du ein Verbrechen begehen wolltest. " +
            "Pass auf, dass er nicht zur Schulleitung geht und dich meldet. " +
            "Versuche deine Beziehung zum Lehrer wieder zu verbessern.";
        
        private readonly TriggerBase Trigger;
        private readonly Player.Player Player;
        private readonly float SchwereMindestens;
        private readonly ISaetzeMoeglichkeitenEinmalig Reaktion;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;

        private bool Fertig;

        public VerbrechenErkennenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Player.Player player,
            float schwereMindestens = 0f,
            ISaetzeMoeglichkeitenEinmalig reaktion = null,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Player = player;
            SchwereMindestens = schwereMindestens;
            Reaktion = reaktion;
            SaetzeWeg = saetzeWeg;
        }

        private bool SiehtVerbrechen()
        {
            return Lehrer.Vision.CanSee(Player.gameObject) && Player.VerbrechenManager.BegehtGeradeEinVerbrechen && Player.VerbrechenManager.Schwere >= SchwereMindestens;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            if (!Trigger.Resolve())
            {
                return false;
            }

            if (currentlyRunning)
            {
                return !Fertig;
            }
            
            return SiehtVerbrechen();
        }

        public override void OnGoalStart()
        {
            Fertig = false;
            Player.Chat.SendChatMessage(WarningMsg);
            Lehrer.Sprache.Say(Reaktion);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Reputation.AddReputation(-Player.VerbrechenManager.Schwere);
            Player.VerbrechenManager.VerbrechenAbbrechen();
            Lehrer.Agent.destination = Player.transform.position;
        }

        public override IEnumerator Execute()
        {
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            yield return new WaitForSeconds(5f);
            Fertig = true;
        }
    }
}
