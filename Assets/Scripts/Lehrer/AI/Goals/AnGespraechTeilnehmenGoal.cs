using System.Collections;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class AnGespraechTeilnehmenGoal : GoalBase
    {
        [SerializeField] private Trigger.Trigger Trigger;
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private Gespraech Gespraech;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAufDemWeg;

        public bool IsAngekommen { get; private set; }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Trigger.Resolve();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAufDemWeg;
            Lehrer.Agent.destination = Standpunkt.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            IsAngekommen = true;
        }

        public Gespraech GetGespraech()
        {
            return Gespraech;
        }
    }
}