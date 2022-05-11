using System;
using System.Collections.Generic;

namespace HerderGames.Zeit
{
    public static class StundenPlanRaster
    {
        public const float SchuleBeginn = 8f;
        public const float EndeDesTages = 24f;

        private static readonly Dictionary<Wochentag, List<StundenPlanEintrag>> Cache = new();

        public static IList<StundenPlanEintrag> GetTagesAblauf(Wochentag wochentag)
        {
            if (Cache.ContainsKey(wochentag))
            {
                return Cache[wochentag];
            }

            var result = new List<StundenPlanEintrag>();

            var time = SchuleBeginn;
            var fachIndex = 0;
            var kurzpauseIndex = 0;
            foreach (var stunde in wochentag.GetAblauf())
            {
                result.Add(new StundenPlanEintrag
                {
                    Tag = wochentag,
                    Beginn = time,
                    Laenge = stunde.GetDuration(),
                    Ende = time + stunde.GetDuration(),
                    Stunde = stunde,
                    FachIndex = fachIndex,
                    KurzpauseIndex = kurzpauseIndex
                });

                time += stunde.GetDuration();

                if (stunde == StundenType.Fach)
                {
                    fachIndex++;
                }

                if (stunde == StundenType.Kurzpause)
                {
                    kurzpauseIndex++;
                }
            }

            Cache[wochentag] = result;
            return result;
        }

        public static int? GetStundenPlanEintragIndexForTime(Wochentag wochentag, float time)
        {
            var ablauf = GetTagesAblauf(wochentag);
            for (var i = 0; i < ablauf.Count; i++)
            {
                var stunde = ablauf[i];

                if (stunde.Beginn <= time && stunde.Ende >= time)
                {
                    return i;
                }
            }

            return null;
        }

        public static StundenPlanEintrag GetStundenPlanEintragForTime(Wochentag wochentag, float time)
        {
            var index = GetStundenPlanEintragIndexForTime(wochentag, time);
            if (index == null)
            {
                return null;
            }

            return GetTagesAblauf(wochentag)[(int) index];
        }

        public static StundenPlanEintrag GetCurrentStundenPlanEintrag(TimeManager timeManager)
        {
            return GetStundenPlanEintragForTime(timeManager.CurrentWochentag, timeManager.CurrentTime);
        }

        public static StundenPlanEintrag GetNaechstenStundenPlanEintrag(Wochentag wochentag, float time, Func<StundenPlanEintrag, bool> predicate)
        {
            var currentStundenDataIndex = GetStundenPlanEintragIndexForTime(wochentag, time);
            if (currentStundenDataIndex == null)
            {
                if (time < SchuleBeginn)
                {
                    currentStundenDataIndex = -1;   
                }
                else
                {
                    return null;
                }
            }
            
            var ablauf = GetTagesAblauf(wochentag);

            for (var i = currentStundenDataIndex + 1; i < ablauf.Count; i++)
            {
                var stunde = ablauf[(int) i];
                if (predicate(stunde))
                {
                    return stunde;
                }
            }

            return null;
        }
    }
}