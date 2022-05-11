using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class VerbrechenMeldenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Player.Player Player;
        private readonly Vector3 SchulleitungsBuero;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public VerbrechenMeldenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Player.Player player,
            Vector3 schulleitungsBuero,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Player = player;
            SchulleitungsBuero = schulleitungsBuero;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve() && Lehrer.Reputation.ShouldGoToSchulleitung();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = SchulleitungsBuero;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            yield return new WaitForSeconds(5);
            Player.Verwarnungen.Add();
            Lehrer.Reputation.ResetAfterMelden();
        }
    }
}
