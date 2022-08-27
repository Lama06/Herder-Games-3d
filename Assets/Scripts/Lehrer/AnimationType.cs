using System;

namespace HerderGames.Lehrer
{
    public enum AnimationType
    {
        IDLE,
        WALKING,
        PAIN
    }

    public static class AnimationTypeExtensions
    {
        public static string AnimationName(this AnimationType type)
        {
            return type switch
            {
                AnimationType.IDLE => "Idle",
                AnimationType.WALKING => "Walking",
                AnimationType.PAIN => "Pain",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
