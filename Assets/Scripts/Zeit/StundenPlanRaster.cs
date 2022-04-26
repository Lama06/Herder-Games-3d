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
                    Duration = stunde.GetDuration(),
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

        public static int GetStundenPlanEintragIndexForTime(Wochentag wochentag, float time)
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

            return -1;
        }

        public static StundenPlanEintrag GetStundenPlanEintragForTime(Wochentag wochentag, float time)
        {
            var index = GetStundenPlanEintragIndexForTime(wochentag, time);
            if (index == -1)
            {
                return null;
            }

            return GetTagesAblauf(wochentag)[index];
        }

        public static StundenPlanEintrag GetCurrentStundenPlanEintrag(TimeManager timeManager)
        {
            return GetStundenPlanEintragForTime(timeManager.GetCurrentWochentag(), timeManager.GetCurrentTime());
        }

        public static StundenPlanEintrag GetNaechstenStundenPlanEintragWithType(Wochentag wochentag, float time,
            StundenType type)
        {
            var currentStundenDataIndex = GetStundenPlanEintragIndexForTime(wochentag, time);
            if (currentStundenDataIndex == -1)
            {
                return null;
            }
            
            var ablauf = GetTagesAblauf(wochentag);

            for (var i = currentStundenDataIndex + 1; i < ablauf.Count; i++)
            {
                var stunde = ablauf[i];
                if (stunde.Stunde == type)
                {
                    return stunde;
                }
            }

            return null;
        }

        public static StundenPlanEintrag GetStundenPlanEintragForFach(Wochentag wochentag, int fachIndex)
        {
            var tagesAblauf = GetTagesAblauf(wochentag);
            foreach (var tagesAblaufEintrag in tagesAblauf)
            {
                if (tagesAblaufEintrag.Stunde == StundenType.Fach && tagesAblaufEintrag.FachIndex == fachIndex)
                {
                    return tagesAblaufEintrag;
                }
            }

            return null;
        }

        public static StundenPlanEintrag GetStundenPlanEintragForKurzpause(Wochentag wochentag, int pauseIndex)
        {
            var tagesAblauf = GetTagesAblauf(wochentag);
            foreach (var tagesAblaufEintrag in tagesAblauf)
            {
                if (tagesAblaufEintrag.Stunde == StundenType.Kurzpause &&
                    tagesAblaufEintrag.KurzpauseIndex == pauseIndex)
                {
                    return tagesAblaufEintrag;
                }
            }

            return null;
        }

        public static IList<StundenPlanEintrag> GetAlleStundenPlanEintraegeWithType(Wochentag wochentag,
            StundenType type)
        {
            var result = new List<StundenPlanEintrag>();
            foreach (var tagesAblaufEintrag in GetTagesAblauf(wochentag))
            {
                if (tagesAblaufEintrag.Stunde == type)
                {
                    result.Add(tagesAblaufEintrag);
                }
            }

            return result;
        }
    }
}