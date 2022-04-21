using System.Collections;
using HerderGames.AI;
using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class BeiAlarmZuSammelpunktGehen : LehrerGoalBase
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
            Lehrer.Sprache.SetSatzSource(SaetzeWeg);
            Lehrer.Agent.destination = Sammelpunkt.position;
            yield return NavMeshUtil.WaitForNavMeshAgentToArrive(Lehrer.Agent);
            Lehrer.Sprache.SetSatzSource(SaetzeAngekommen);
        }
    }
}