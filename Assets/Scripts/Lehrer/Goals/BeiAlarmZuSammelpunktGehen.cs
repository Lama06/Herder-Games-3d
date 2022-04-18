using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class BeiAlarmZuSammelpunktGehen : LehrerGoalBase
    {
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Sammelpunkt;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return AlarmManager.IsAlarm();
        }

        public override void OnStarted()
        {
            Lehrer.GetAgent().destination = Sammelpunkt.position;
        }
    }
}