namespace HerderGames.Zeit
{
    public static class TimeUtility
    {
        public static float MinutesToHours(float min)
        {
            return min / 60f;
        }

        public static float HoursToMinutes(float f)
        {
            return f * 60f;
        }
        
        public static string FormatTime(float time)
        {
            var hours = (int) time;
            var hoursText = hours.ToString();
            if (hoursText.Length == 1)
            {
                hoursText = "0" + hoursText;
            }

            var minutes = (int) HoursToMinutes(time - hours);
            var minutesText = minutes.ToString();
            if (minutesText.Length == 1)
            {
                minutesText = "0" + minutesText;
            }

            return hoursText + ":" + minutesText;
        }
    }
}
