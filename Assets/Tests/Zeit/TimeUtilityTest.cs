using HerderGames.Zeit;
using NUnit.Framework;

namespace HerderGames.Tests.Zeit
{
    public class TimeUtilityTest
    {
        [Test]
        public void TestMinutesToHours()
        {
            Assert.AreEqual(30f.MinutesToHours(), 0.5f);
            Assert.AreEqual(10f.MinutesToHours(), 10f/60f);
            Assert.AreEqual(120f.MinutesToHours(), 2f);
        }

        [Test]
        public void TestHoursToMinutes()
        {
            Assert.AreEqual(1f.HoursToMinutes(), 60f);
            Assert.AreEqual(2f.HoursToMinutes(), 120f);
            Assert.AreEqual(0.5f.HoursToMinutes(), 30f);
            Assert.AreEqual(0.75f.HoursToMinutes(), 45f);
        }

        [Test]
        public void TestFormatTime()
        {
            Assert.AreEqual(12f.FormatTime(), "12:00");
            Assert.AreEqual(24f.FormatTime(), "24:00");
            Assert.AreEqual(12.5f.FormatTime(), "12:30");
            Assert.AreEqual(12.75f.FormatTime(), "12:45");
            Assert.AreEqual(3.75f.FormatTime(), "03:45");
            Assert.AreEqual(0f.FormatTime(), "00:00");
        }
    }
}
