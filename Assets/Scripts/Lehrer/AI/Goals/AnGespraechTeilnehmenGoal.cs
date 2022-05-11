using System.Collections;
using HerderGames.Lehrer.AI.Trigger;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class AnGespraechTeilnehmenGoal : GoalBase
    {
        private readonly TriggerBase Trigger;
        private readonly Vector3 Standpunkt;
        private readonly Gespraech Gespraech;
        private readonly ISaetzeMoeglichkeitenMehrmals SaetzeAufDemWeg;

        public bool IsAngekommen { get; private set; }

        public AnGespraechTeilnehmenGoal(
            Lehrer lehrer,
            TriggerBase trigger,
            Vector3 standpunkt,
            Gespraech gespraech,
            ISaetzeMoeglichkeitenMehrmals saetzeAufDemWeg = null
        ) : base(lehrer)
        {
            Trigger = trigger;
            Standpunkt = standpunkt;
            Gespraech = gespraech;
            SaetzeAufDemWeg = saetzeAufDemWeg;
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override void OnGoalEnd(GoalEndReason reason)
        {
            IsAngekommen = false;
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWeg;
            Lehrer.Agent.destination = Standpunkt;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            IsAngekommen = true;
        }

        public Gespraech GetGespraech()
        {
            return Gespraech;
        }
    }
}
