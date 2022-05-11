using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class MoveToAndStandAtGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Vector3 Position;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeWeg;
        private readonly ISaetzeMoeglichkeitenEinmalig SaetzeAngekommenEinmalig;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public MoveToAndStandAtGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 position,
            ISaetzeMoeglichkeitenMehrmals saetzeWeg = null,
            ISaetzeMoeglichkeitenMehrmals saetzeAngekommen = null,
            ISaetzeMoeglichkeitenEinmalig saetzeAngekommenEinmalig = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Position = position;
            SaetzeWeg = saetzeWeg;
            SaetzeAngekommenEinmalig = saetzeAngekommenEinmalig;
            SaetzeAngekommen = saetzeAngekommen;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.Say(SaetzeAngekommenEinmalig);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}
