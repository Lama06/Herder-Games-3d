using HerderGames.Util;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Schule
{
    public class AlarmManager : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;
        [SerializeField] private Dauer LaengeDesAlarms;
        [SerializeField] private int SchadenFuerDieSchule;
        
        private Zeitpunkt AlarmStartZeitpunkt;

        public bool IsAlarm => AlarmStartZeitpunkt != null && LaengeDesAlarms.IsLongerThan(TimeManager.CurrentZeitpunkt.Diff(AlarmStartZeitpunkt));

        public void AlarmStarten()
        {
            AlarmStartZeitpunkt = TimeManager.CurrentZeitpunkt;
            Player.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
            Player.Chat.SendChatMessageDurchsage("Achtung, das ist ist kein Probealarm. In der Schule gibt es einen Brand.");
        }

        public void LoadData()
        {
            var startZeitpunktExists = PlayerPrefsUtil.GetBool("alarm.startzeit_exists", false);
            if (!startZeitpunktExists)
            {
                AlarmStartZeitpunkt = null;
                return;
            }
            
            AlarmStartZeitpunkt = PlayerPrefsUtil.GetZeitpunkt("alarm.startzeit", new Zeitpunkt());
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetBool("alarm.startzeit_exists", AlarmStartZeitpunkt != null);
            if (AlarmStartZeitpunkt != null)
            {
                PlayerPrefsUtil.SetZeitpunkt("alarm.startzeit", AlarmStartZeitpunkt);
            }
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey("alarm.startzeit_exists");
            PlayerPrefsUtil.DeleteZeitpunkt("alarm.startzeit");
        }
    }
}
