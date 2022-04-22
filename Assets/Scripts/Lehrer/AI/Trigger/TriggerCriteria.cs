using System;
using HerderGames.Schule;
using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Trigger
{
    [Serializable]
    public class TriggerCriteria
    {
        public TriggerType Type;
        public bool Umkehren;

        [Header("Zeit")] public TimeManager TimeManager;
        public WoechentlicheZeitspannen Wann;

        [Header("Alarm")] public AlarmManager Alarm;

        [Header("Vergiftung")] public VergiftungsManager Vergiftung;

        public bool Resolve()
        {
            var result = Type switch
            {
                TriggerType.Zeit => Wann.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime()),
                TriggerType.Alarm => Alarm.IsAlarm(),
                TriggerType.Vergiftung => Vergiftung.Syntome,
                _ => throw new ArgumentOutOfRangeException()
            };

            return Umkehren ? !result : result;
        }
    }
}