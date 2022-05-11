using HerderGames.Zeit;

namespace HerderGames.Lehrer.AI.Trigger
{
    public class ZeitspanneTrigger : TriggerBase
    {
        private readonly TimeManager TimeManager;
        private readonly WoechentlicheZeitspannen Zeitspannen;

        public ZeitspanneTrigger(TimeManager timeManager, WoechentlicheZeitspannen zeitspannen)
        {
            TimeManager = timeManager;
            Zeitspannen = zeitspannen;
        }

        public override bool Resolve()
        {
            return Zeitspannen.IsInside(TimeManager.CurrentWochentag, TimeManager.CurrentTime);
        }
    }
}