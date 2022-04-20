using HerderGames.Lehrer.Sprache;
using HerderGames.Schule;
using UnityEngine;

namespace HerderGames.Lehrer.Goals
{
    public class BeiAlarmZuSammelpunktGehen : LehrerGoalBase
    {
        [SerializeField] private AlarmManager AlarmManager;
        [SerializeField] private Transform Sammelpunkt;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals Saetze;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return AlarmManager.IsAlarm();
        }

        public override void OnStarted()
        {
            Lehrer.Agent.destination = Sammelpunkt.position;
            Lehrer.Sprache.SetSatzSource(Saetze);
        }
    }
}