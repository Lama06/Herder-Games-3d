using System;

namespace HerderGames.Zeit
{
    public enum AnfangOderEnde
    {
        Anfang,
        Ende
    }

    public static class AnfangOderEndeExtensions
    {
        public static float ForStunde(this AnfangOderEnde anfangEnde, StundenPlanEintrag eintrag)
        {
            return anfangEnde switch
            {
                AnfangOderEnde.Anfang => eintrag.Beginn,
                AnfangOderEnde.Ende => eintrag.Ende,
                _ => throw new ArgumentOutOfRangeException(nameof(anfangEnde), anfangEnde, null)
            };
        }
    }
}