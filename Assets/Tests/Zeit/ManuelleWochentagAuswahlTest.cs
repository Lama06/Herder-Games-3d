using System.Collections.Generic;
using HerderGames.Zeit;
using NUnit.Framework;

namespace HerderGames.Tests.Zeit
{
    public class ManuelleWochentagAuswahlTest
    {
        [Test]
        public void Test() 
        {
            Assert.AreEqual(new ManuelleWochentagAuswahl(Wochentag.Montag, Wochentag.Dienstag).Wochentage,
                new HashSet<Wochentag> {Wochentag.Montag, Wochentag.Dienstag});
            Assert.AreEqual(new ManuelleWochentagAuswahl(Wochentag.Samstag, Wochentag.Mittwoch).Wochentage,
                new HashSet<Wochentag> {Wochentag.Samstag, Wochentag.Mittwoch});
        }
    }
}
