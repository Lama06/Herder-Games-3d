using HerderGames.AI;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    [RequireComponent(typeof(Lehrer))]
    public class BeiAlarmAufDenSchulhofGehen : GoalBase
    {
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Sammelpunkt;
        
        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public override bool ShouldRun(bool currentlyRunning)
        {
            return AlarmManager.Alarm;
        }

        public override void OnStarted()
        {
            Lehrer.GetAgent().destination = Sammelpunkt.position;
        }
    }
}