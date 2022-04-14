using HerderGames.Time.Stundenplan;
using UnityEngine;

namespace HerderGames.Time
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float TimeSpeed;
        [SerializeField] private Wochentag CurrentWochentag = Wochentag.Montag;
        [SerializeField] private float CurrentTime = StundenPlanRaster.SchuleBeginn;

        public Wochentag GetCurrentWochentag()
        {
            return CurrentWochentag;
        }

        public float GetCurrentTime()
        {
            return CurrentTime;
        }

        private void Update()
        {
            CurrentTime += TimeSpeed * UnityEngine.Time.deltaTime;
            if (CurrentTime > StundenPlanRaster.EndeDesTages)
            {
                CurrentTime = 0;
                CurrentWochentag = CurrentWochentag.GetNextWochentag();
            }
        }
    }
}