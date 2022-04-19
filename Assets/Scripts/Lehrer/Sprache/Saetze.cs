using System.Collections.Generic;

namespace HerderGames.Lehrer.Sprache
{
    [System.Serializable]
    public class Saetze
    {
        public Satz[] MoeglicheSaetze;
        public string[] SharedIds;

        public List<Satz> Resolve(SpracheManager manager)
        {
            var result = new List<Satz>();
            result.AddRange(MoeglicheSaetze);

            foreach (var shared in SharedIds)
            {
                result.AddRange(manager.GetShared(shared));
            }

            return result;
        }
    }
}