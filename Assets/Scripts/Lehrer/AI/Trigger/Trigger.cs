using System;

namespace HerderGames.Lehrer.AI.Trigger
{
    [Serializable]
    public class Trigger
    {
        public TriggerCriteria[] Bedingungen;

        public bool Resolve()
        {
            var result = true;

            foreach (var bedingung in Bedingungen)
            {
                if (!bedingung.Resolve())
                {
                    result = false;
                }
            }

            return result;
        }
    }
}