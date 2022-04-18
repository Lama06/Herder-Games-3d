using System.Collections;
using HerderGames.AI;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class VergiftbaresEssenEssenGehen : GoalBase
    {
        [SerializeField] private VergiftbaresEssen VergiftbaresEssen;
        [SerializeField] private WoechentlicheZeitspannen Wann;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return VergiftbaresEssen.Status != VergiftungsStatus.VergiftetBemerkt && Wann.IsInside(Lehrer.GetTimeManager().GetCurrentWochentag(), Lehrer.GetTimeManager().GetCurrentTime());
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
