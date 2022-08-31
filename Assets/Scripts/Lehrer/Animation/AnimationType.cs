using System;

namespace HerderGames.Lehrer.Animation
{
    public enum AnimationType
    {
        Stehen,
        GehenKrank,
        Gehen,
        Rauchen,
        RedenAggressiv,
        Reden,
        Rueckenschmerzen,
        SchmerzBoden,
        Winken,
        Trinken,
        BetrunkenRennen,
        Aggressiv
    }

    public static class AnimationTypeExtensions
    {
        public static string ParameterName(this AnimationType animationType)
        {
            return animationType switch
            {
                AnimationType.Stehen => "Stehen",
                AnimationType.GehenKrank => "Gehen Krank",
                AnimationType.Gehen => "Gehen",
                AnimationType.Rauchen => "Rauchen",
                AnimationType.RedenAggressiv => "Reden Aggressiv",
                AnimationType.Reden => "Reden",
                AnimationType.Rueckenschmerzen => "Rückenschmerzen",
                AnimationType.SchmerzBoden => "Schmerz Boden",
                AnimationType.Winken => "Winken",
                AnimationType.Trinken => "Trinken",
                AnimationType.BetrunkenRennen => "Betrunken Rennen",
                AnimationType.Aggressiv => "Aggressiv",
                _ => throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null)
            };
        }
    }
}
