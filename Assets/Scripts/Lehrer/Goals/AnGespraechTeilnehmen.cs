using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class AnGespraechTeilnehmen : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private Gespraech Gespraech;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAufDemWeg;

        public bool IsAngekommen { get; private set; }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SetSatzSource(SaetzeAufDemWeg);
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