using System;
using System.Linq;
using HerderGames.Zeit;
using NUnit.Framework;

namespace HerderGames.Tests.Zeit
{
    public class EigenschaftWochentagAuswahlTest
    {
        [Test]
        public void Test()
        {
            var eigenschaften = Enum.GetValues(typeof(WochentagEigenschaft)).Cast<WochentagEigenschaft>();
            foreach (var eigenschaft in eigenschaften)
            {
                Assert.AreEqual(eigenschaft.GetWochentage(), new EigenschaftWochentagAuswahl(eigenschaft).Wochentage);
            }
        }
    }
}
