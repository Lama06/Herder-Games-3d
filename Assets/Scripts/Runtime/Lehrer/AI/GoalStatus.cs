using System;

namespace HerderGames.Lehrer.AI
{
    public abstract class GoalStatus
    {
        public class CanStartIf : GoalStatus
        {
            public bool Value { get; }

            public CanStartIf(bool value)
            {
                Value = value;
            }
        }
        
        public class ContinueIf : GoalStatus
        {
            public bool Value { get; }

            public ContinueIf(bool value)
            {
                Value = value;
            }
        }

        public class Continue : GoalStatus { }

        public bool ShouldContinue()
        {
            return this switch
            {
                Continue => true,
                ContinueIf condition => condition.Value,
                _ => throw new Exception()
            };
        }
    }
}