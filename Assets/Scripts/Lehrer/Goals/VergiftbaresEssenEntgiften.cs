using System.Collections;
using HerderGames.AI;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class VergiftbaresEssenEntgiften : GoalBase
    {
        [SerializeField] private WoechentlicheZeitspannen Wann;
        [SerializeField] private VergiftbaresEssen Essen;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Essen.Status == VergiftungsStatus.VergiftetBemerkt &&
                   Wann.IsInside(Lehrer.GetTimeManager().GetCurrentWochentag(),
                       Lehrer.GetTimeManager().GetCurrentTime());
        }

        public override IEnumerator Execute()
        {
            Lehrer.GetAgent().destination = Essen.GetStandpunkt();
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
            Essen.Status = VergiftungsStatus.NichtVergiftet;
        }
    }
}