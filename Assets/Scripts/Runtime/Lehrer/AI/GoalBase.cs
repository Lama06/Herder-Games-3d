using System;
using System.Collections;
using System.Collections.Generic;

namespace HerderGames.Lehrer.AI
{
    public abstract class GoalBase
    {
        public Lehrer Lehrer { get; }

        protected GoalBase(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }

        public abstract IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback);

        public virtual IEnumerable UpdateGoal()
        {
            yield break;
        }
    }
}