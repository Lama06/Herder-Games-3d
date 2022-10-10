using System;

namespace HerderGames.Lehrer.Animation
{
    public enum AnimationType
    {
        Stehend,
        Sitzend
    }

    public static class AnimationTypeExtensions
    {
        public static string ParameterName(this AnimationType animationType)
        {
            return animationType switch
            {
                AnimationType.Stehend => "Stehend",
                AnimationType.Sitzend => "Sitzend",
                _ => throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null)
            };
        }
    }
}