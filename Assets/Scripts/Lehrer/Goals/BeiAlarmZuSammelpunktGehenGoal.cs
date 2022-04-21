using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class BeiAlarmZuSammelpunktGehenGoal : LehrerGoalBase
    {
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Sammelpunkt;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeWeg;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals SaetzeAngekommen;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return AlarmManager.IsAlarm();
        }

        public override IEnumerator Execute()
        {
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeWeg;
            Lehrer.Agent.destination = Sammelpunkt.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.SaetzeMoeglichkeiten = SaetzeAngekommen;
        }
    }
}