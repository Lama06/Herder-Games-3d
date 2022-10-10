using System;
using System.Text;

namespace HerderGames.Util
{
    public static class Utility
    {
        private static readonly Random Random = new();

        public static bool TrueWithPercent(float percentForTrue)
        {
            var randomNumber = UnityEngine.Random.Range(0f, 1f);
            return randomNumber <= percentForTrue;
        }

        public static string ZufaelligGrossKlein(this string text)
        {
            var builder = new StringBuilder();
            foreach (var c in text)
            {
                // Hier System.Random benutzen, weil diese Methode oft in einem Konstruktor aufgerufen wird, wo UnityEngine.Random nicht funktioniert
                builder.Append(Random.Next(2) == 0 ? char.ToLower(c) : char.ToUpper(c));
            }

            return builder.ToString();
        }
    }
}
