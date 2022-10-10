using System;
using System.Collections.Generic;
using System.Linq;

namespace HerderGames.Zeit
{
    public class PredicateWochentagAuswahl : WochentagAuswahlBase
    {
        private readonly Func<Wochentag, bool> Predicate;

        public PredicateWochentagAuswahl(Func<Wochentag, bool> predicate)
        {
            Predicate = predicate;
        }

        public override ISet<Wochentag> Wochentage
        {
            get
            {
                var result = new HashSet<Wochentag>();

                foreach (var tag in Enum.GetValues(typeof(Wochentag)).Cast<Wochentag>())
                {
                    if (Predicate(tag))
                    {
                        result.Add(tag);
                    }
                }

                return result;
            }
        }
    }
}