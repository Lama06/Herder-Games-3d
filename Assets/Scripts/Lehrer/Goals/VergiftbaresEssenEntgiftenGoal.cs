using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VergiftbaresEssenEntgiftenGoal : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private VergiftbaresEssen Essen;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Essen.Status == VergiftungsStatus.VergiftetBemerkt &&
                   Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Essen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Essen.Status = VergiftungsStatus.NichtVergiftet;
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}