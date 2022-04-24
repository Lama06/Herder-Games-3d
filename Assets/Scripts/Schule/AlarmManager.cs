using HerderGames.Time;
using UnityEngine;

namespace HerderGames.Schule
{
    public class AlarmManager : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private ZeitDauer LaengeDesAlarms;
        [SerializeField] private int SchadenFuerDieSchule;

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
            Player.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
            Player.Chat.SendChatMessageDurchsage(
                "Achtung, das ist ist kein Probealarm. In der Schule gibt es einen Brand.");
        }
    }
}