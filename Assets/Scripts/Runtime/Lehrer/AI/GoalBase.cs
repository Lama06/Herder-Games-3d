using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    public abstract class GoalBase
    {
        public Lehrer Lehrer { get; private set; }

        public void InitLehrer(Lehrer lehrer)
        {
            if (Lehrer != null)
            {
                throw new Exception($"{nameof(InitLehrer)} called twice");
            }
            
            Lehrer = lehrer;
        }
        
        public abstract IEnumerable ExecuteGoal(Stack<Action> unexpectedGoalEndCallback);

        public virtual IEnumerable UpdateGoal()
        {
            yield break;
        }
    }
}