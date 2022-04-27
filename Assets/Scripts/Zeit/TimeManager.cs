using System.Text;
using UnityEngine;

namespace HerderGames.Zeit
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float TimeSpeed;
        private int CurrentKalenderwoche = 1;
        private Wochentag CurrentWochentag = Wochentag.Montag;
        private float CurrentTime = 7f;

        public int GetCurrentKalenderwoche()
        {
            return CurrentKalenderwoche;
        }
        
        public Wochentag GetCurrentWochentag()
        {
            return CurrentWochentag;
        }

        public float GetCurrentTime()
        {
            return CurrentTime;
        }

        public Zeitpunkt GetCurrentZeitpunkt()
        {
            return new Zeitpunkt
            {
                Kalenderwoche = CurrentKalenderwoche,
                Wochentag = CurrentWochentag,
                Time = CurrentTime
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
            CurrentTime += TimeSpeed * UnityEngine.Time.deltaTime;
            if (CurrentTime >= StundenPlanRaster.EndeDesTages)
            {
                CurrentTime = 0;
                CurrentWochentag += 1;
            }

            if (!CurrentWochentag.IsValid())
            {
                CurrentKalenderwoche++;
                CurrentWochentag = Wochentag.Montag;
            }
        }
    }
}