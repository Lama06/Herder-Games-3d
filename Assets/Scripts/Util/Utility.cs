using UnityEngine;

namespace HerderGames.Util
{
    public static class Utility
    {
        public static bool TrueWithPercent(float percentForTrue)
        {
            var randomNumber = Random.Range(0f, 1f);
            return randomNumber <= percentForTrue;
        }
    }
}