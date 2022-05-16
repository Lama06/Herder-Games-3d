using System.Text;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Zeit
{
    public class TimeManager : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private float TimeSpeed;
        [SerializeField] private float TimeSpeedUnterricht;
        [SerializeField] private int StartKalenderwoche;
        [SerializeField] private Wochentag StartWochentag;
        [SerializeField] private float StartZeit;

        public int CurrentKalenderwoche { get; set; }
        public Wochentag CurrentWochentag { get; set; }
        public float CurrentTime { get; set; }

        public Zeitpunkt GetCurrentZeitpunkt()
        {
            return new Zeitpunkt
            {
                Kalenderwoche = CurrentKalenderwoche,
                Wochentag = CurrentWochentag,
                Zeit = CurrentTime
            };
        }

        public string GetDisplayText()
        {
            var builder = new StringBuilder();
            builder.Append("Woche: ").Append(CurrentKalenderwoche).Append("\n");
            builder.Append("Wochentag: ").Append(CurrentWochentag).Append("\n");
            builder.Append("Zeit: ").Append(TimeUtility.FormatTime(CurrentTime));
            return builder.ToString();
        }

        private void Update()
        {
            CurrentTime += (StundenPlanRaster.GetCurrentStundenPlanEintrag(this) is {Stunde: StundenType.Fach} ? TimeSpeedUnterricht : TimeSpeed) * Time.deltaTime;
            if (CurrentTime >= StundenPlanRaster.EndeDesTages)
            {
                CurrentTime = 0;
                CurrentWochentag += 1;
                
                if (!CurrentWochentag.IsValid())
                {
                    CurrentKalenderwoche++;
                    CurrentWochentag = Wochentag.Montag;
                }
            }
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("time.kalenderwoche", CurrentKalenderwoche);
            PlayerPrefs.SetInt("time.wochentag", (int) CurrentWochentag);
            PlayerPrefs.SetFloat("time.time", CurrentTime);
        }

        public void LoadData()
        {
            CurrentKalenderwoche = PlayerPrefs.GetInt("time.kalenderwoche", StartKalenderwoche);
            CurrentWochentag = (Wochentag) PlayerPrefs.GetInt("time.wochentag", (int) StartWochentag);
            CurrentTime = PlayerPrefs.GetFloat("time.time", StartZeit);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey("time.kalenderwoche");
            PlayerPrefs.DeleteKey("time.wochentag");
            PlayerPrefs.DeleteKey("time.time");
        }
    }
}
