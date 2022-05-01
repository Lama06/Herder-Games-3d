using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    [RequireComponent(typeof(Lehrer))]
    public class AIController : MonoBehaviour
    { 
        public List<GoalBase> Goals { get; } = new();
        public GoalBase CurrentGoal { get; private set; }

        public void AddGoal(GoalBase goal)
        {
            Goals.Add(goal);
            goal.OnGoalEnable();
        }

        private void Update()
        {
            if (CurrentGoal != null && !CurrentGoal.ShouldRun(true))
            {
                CurrentGoal.EndGoal(GoalEndReason.GoalCannotContinue);
                CurrentGoal = null;
            }

            var newGoal = GetGoalWithHighestPriorityThatCanStart();
            if (newGoal == null || newGoal == CurrentGoal)
            {
                return;
            }

            if (CurrentGoal != null)
            {
                CurrentGoal.EndGoal(GoalEndReason.OtherGoalWithHigherPriority);
            }

            CurrentGoal = newGoal;
            CurrentGoal.StartGoal();
        }

        private GoalBase GetGoalWithHighestPriorityThatCanStart()
        {
            GoalBase goalWithHighestPriority = null;
            
            for (var i = Goals.Count - 1; i >= 0; i--)
            {
                var goal = Goals[i];
                
                if (!goal.ShouldRun(false))
                {
                    continue;
                }
                
                goalWithHighestPriority = goal;
            }

            return goalWithHighestPriority;
        }
    }
}