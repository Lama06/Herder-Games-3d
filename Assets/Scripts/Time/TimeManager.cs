using UnityEngine;

namespace HerderGames.Time
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float TimeSpeed;
        private int CurrentKalenderwoche = 1;
        private Wochentag CurrentWochentag = Wochentag.Montag;
        private float CurrentTime = StundenPlanRaster.SchuleBeginn;

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

        private void Update()
        {
            CurrentTime += TimeSpeed * UnityEngine.Time.deltaTime;
            if (CurrentTime > StundenPlanRaster.EndeDesTages)
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