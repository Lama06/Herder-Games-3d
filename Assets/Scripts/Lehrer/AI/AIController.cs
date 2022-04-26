using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    [RequireComponent(typeof(Lehrer))]
    public class AIController : MonoBehaviour
    {
        private const int GoalPriorityNoGoal = -1;
        
        public List<GoalBase> Goals { get; } = new();
        private int CurrentGoalPriority = GoalPriorityNoGoal;
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
                CurrentGoalPriority = GoalPriorityNoGoal;
            }

            var (newGoal, newGoalPriority) = GetGoalWithHigherPriorityThatCanStart();
            if (newGoal == null)
            {
                return;
            }

            if (CurrentGoal != null)
            {
                CurrentGoal.EndGoal(GoalEndReason.OtherGoalWithHigherPriority);
            }

            CurrentGoal = newGoal;
            CurrentGoal.StartGoal();
            CurrentGoalPriority = newGoalPriority;
        }

        private (GoalBase, int) GetGoalWithHigherPriorityThatCanStart()
        {
            var priority = -1;
            for (var i = Goals.Count - 1; i >= 0; i--)
            {
                priority++;
                if (priority <= CurrentGoalPriority)
                {
                    continue;
                }

                var goal = Goals[i];

                if (!goal.ShouldRun(false))
                {
                    continue;
                }

                return (goal, priority);
            }

            return (null, GoalPriorityNoGoal);
        }
    }
}