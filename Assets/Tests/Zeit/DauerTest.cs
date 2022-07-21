using HerderGames.Zeit;
using NUnit.Framework;

namespace HerderGames.Tests.Zeit
{
    public class DauerTest
    {
        [Test]
        public void TestFromStunden()
        {
            void Test(float stundenIn, int wochenOut, int tageOut, float stundenOut)
            {
                var dauer = Dauer.FromStunden(stundenIn);
                Assert.AreEqual(dauer.Wochen, wochenOut);
                Assert.AreEqual(dauer.Tage, tageOut);
                Assert.AreEqual(dauer.Stunden, stundenOut);
            }

            Test(1f, 0, 0, 1f);
            Test(1.5f, 0, 0, 1.5f);
            Test(23f, 0, 0, 23f);
            Test(24f, 0, 1, 0f);
            Test(25.5f, 0, 1, 1.5f);
            Test(49f, 0, 2, 1f);
            Test(7f * 24f, 1, 0, 0f);
            Test(2f * 7f * 24f + 3f * 24f + 7.5f, 2, 3, 7.5f);
        }

        [Test]
        public void TestLaenge()
        {
            Assert.AreEqual(Dauer.FromStunden(1f).Laenge, 1f);
            Assert.AreEqual(Dauer.FromStunden(10f).Laenge, 10f);
            Assert.AreEqual(Dauer.FromStunden(1801f).Laenge, 1801f);
        }

        [Test]
        public void TestComparisions()
        {
            void AsserOrder(Dauer small, Dauer large)
            {
                Assert.IsTrue(small < large);
                Assert.IsTrue(!(small > large));
            }
            
            AsserOrder(Dauer.FromStunden(1f), Dauer.FromStunden(2f));
            AsserOrder(Dauer.FromStunden(25f), Dauer.FromStunden(100f));
            AsserOrder(Dauer.FromStunden(9999f), Dauer.FromStunden(10000f));
        }
    }
}
