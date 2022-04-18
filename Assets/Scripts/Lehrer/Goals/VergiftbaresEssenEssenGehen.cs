using System.Collections;
using HerderGames.AI;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VergiftbaresEssenEssenGehen : LehrerGoalBase
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private VergiftbaresEssen VergiftbaresEssen;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.GetAgent().destination = VergiftbaresEssen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
            if (VergiftbaresEssen.Status.IsVergiftet())
            {
                Lehrer.GetKrankheit().Erkranken(VergiftbaresEssen);
            }
        }
    }
}
