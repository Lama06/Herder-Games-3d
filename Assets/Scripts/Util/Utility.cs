using System;
using System.Text;
using Random = UnityEngine.Random;

namespace HerderGames.Util
{
    public static class Utility
    {
        public static bool TrueWithPercent(float percentForTrue)
        {
            var randomNumber = Random.Range(0f, 1f);
            return randomNumber <= percentForTrue;
        }

        public static string ZufaelligGrossKlein(this string text)
        {
            var builder = new StringBuilder();
            foreach (var c in text)
            {
                builder.Append(Random.Range(0, 2) == 0 ? char.ToLower(c) : char.ToUpper(c));
            }
            return builder.ToString();
        }
    }
}