using System;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Schule
{
    public class AlarmManager : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private ZeitDauer LaengeDesAlarms;
        
        private bool Alarm;
        private Zeitpunkt AlarmStartZeitpunkt;

        private void Update()
        {
            if (Alarm && TimeManager.GetCurrentZeitpunkt().Diff(AlarmStartZeitpunkt).IsLongerThan(LaengeDesAlarms))
            {
                Alarm = false;
                AlarmStartZeitpunkt = null;
            }
        }

        public bool IsAlarm()
        {
            return Alarm;
        }

        public void AlarmStarten()
        {
            Alarm = true;
            AlarmStartZeitpunkt = TimeManager.GetCurrentZeitpunkt();
        }
    }
}