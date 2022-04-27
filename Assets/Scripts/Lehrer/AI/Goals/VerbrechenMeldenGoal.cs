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
        private readonly Transform SchulleitungsBuero;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly SaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public VerbrechenMeldenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Player.Player player,
            Transform schulleitungsBuero,
            SaetzeMoeglichkeitenMehrmals saetzeWeg,
            SaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig,
            SaetzeMoeglichkeitenMehrmals saetzeAngekommen
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
            Lehrer.Agent.destination = SchulleitungsBuero.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            yield return new WaitForSeconds(5);
            Player.Verwarnungen.Add();
            Lehrer.Reputation.ResetAfterMelden();
        }
    }
}
