using System;

namespace HerderGames.Lehrer.Animation
{
    public enum AnimationName
    {
        _StehendeAnimationenStart,
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
        Aggressiv,
        _StehendeAnimationenEnde,

        _SitzendeAnimationenStart,
        Sitzen,
        Tippen,
        RedenSitzen,
        AggressivSitzen,
        _SitzendeAnimationenEnde
    }

    public static class AnimationNameExtensions
    {
        public static string ParameterName(this AnimationName animationName)
        {
            return animationName switch
            {
                AnimationName.Stehen => "Stehen",
                AnimationName.GehenKrank => "Gehen Krank",
                AnimationName.Gehen => "Gehen",
                AnimationName.Rauchen => "Rauchen",
                AnimationName.RedenAggressiv => "Reden Aggressiv",
                AnimationName.Reden => "Reden",
                AnimationName.Rueckenschmerzen => "Rückenschmerzen",
                AnimationName.SchmerzBoden => "Schmerz Boden",
                AnimationName.Winken => "Winken",
                AnimationName.Trinken => "Trinken",
                AnimationName.BetrunkenRennen => "Betrunken Rennen",
                AnimationName.Aggressiv => "Aggressiv",
                AnimationName.Sitzen => "Sitzen",
                AnimationName.Tippen => "Tippen",
                AnimationName.RedenSitzen => "Reden Sitzen",
                AnimationName.AggressivSitzen => "Aggressiv Sitzen",
                AnimationName._StehendeAnimationenStart or AnimationName._StehendeAnimationenEnde or AnimationName._SitzendeAnimationenEnde
                    or AnimationName._SitzendeAnimationenEnde => throw new ArgumentException(),
                _ => throw new ArgumentOutOfRangeException(nameof(animationName), animationName, null)
            };
        }

        public static AnimationType AnimationType(this AnimationName animationName)
        {
            return animationName switch
            {
                > AnimationName._StehendeAnimationenStart and < AnimationName._StehendeAnimationenEnde => Animation.AnimationType.Stehend,
                > AnimationName._SitzendeAnimationenStart and < AnimationName._SitzendeAnimationenEnde => Animation.AnimationType.Sitzend,
                _ => throw new ArgumentException()
            };
        }
    }
}