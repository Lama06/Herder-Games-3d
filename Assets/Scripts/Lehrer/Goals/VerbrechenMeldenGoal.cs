using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class VerbrechenMeldenGoal : LehrerGoalBase
    {
        [SerializeField] private Player.Player Player;
        [SerializeField] private Transform SchulleitungsBuero;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return Lehrer.Reputation.ShouldGoToSchulleitung();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = SchulleitungsBuero.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
            yield return new WaitForSeconds(5);
            Player.Verwarnungen.Add();
            Lehrer.Reputation.ResetAfterMelden();
        }
    }
}
