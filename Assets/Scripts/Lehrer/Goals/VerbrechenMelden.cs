using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VerbrechenMelden : LehrerGoalBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private Transform SchulleitungsBuero;
        [SerializeField] private Saetze SaetzeWeg;
        [SerializeField] private Saetze SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Lehrer.Reputation.ShouldGoToSchulleitung();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SetSatzSource(SaetzeWeg);
            Lehrer.Agent.destination = SchulleitungsBuero.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.SetSatzSource(SaetzeAngekommen);
            yield return new WaitForSeconds(5);
            Player.Verwarnungen.Add();
            Lehrer.Reputation.ResetAfterMelden();
        }
    }
}
