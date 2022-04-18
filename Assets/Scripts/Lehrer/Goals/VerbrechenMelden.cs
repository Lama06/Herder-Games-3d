using System.Collections;
using HerderGames.AI;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VerbrechenMelden : LehrerGoalBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private Transform SchulleitungsBuero;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Lehrer.GetReputation().ShouldGoToSchulleitung();
        }

        public override IEnumerator Execute()
        {
            Lehrer.GetAgent().destination = SchulleitungsBuero.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.GetAgent());
            yield return new WaitForSeconds(5);
            Player.GetVerwarnungen().Add();
            Lehrer.GetReputation().ResetAfterMelden();
        }
    }
}
