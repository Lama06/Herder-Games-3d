using System;

namespace HerderGames.Time
{
    public enum StundenType
    {
        Fach,
        Lernzeit,
        Kurzpause,
        Mittagspause
    }

    public static class StundenTypeExtensions
    {
        public static float GetDuration(this StundenType type)
        {
            return type switch
            {
                StundenType.Fach => 90f / 60f,
                StundenType.Lernzeit => 45f / 60f,
                StundenType.Kurzpause => 25f / 60f,
                StundenType.Mittagspause => 60f / 60f,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}