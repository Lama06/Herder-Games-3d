using System;

namespace HerderGames.Lehrer.AI.Trigger
{
    public class CallbackTrigger : TriggerBase
    {
        private readonly Func<bool> Callback;

        public CallbackTrigger(Func<bool> callback)
        {
            Callback = callback;
        }

        public override bool Resolve()
        {
            return Callback();
        }
    }
}