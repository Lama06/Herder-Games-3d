using System.Collections;
using HerderGames.AI;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VergiftbaresEssenEntgiften : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private VergiftbaresEssen Essen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Essen.Status == VergiftungsStatus.VergiftetBemerkt &&
                   Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.GetAgent().destination = Essen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
            Essen.Status = VergiftungsStatus.NichtVergiftet;
        }
    }
}