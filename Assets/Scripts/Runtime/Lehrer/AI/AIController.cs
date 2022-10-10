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
            var mostImportantGoal = GoalWithHighestPriorityThatCanStart;

            if (mostImportantGoal == CurrentGoal)
            {
                return;
            }

            CurrentGoal?.EndGoal();
            CurrentGoal = mostImportantGoal;
            CurrentGoal?.StartGoal();
        }

        private GoalBase GoalWithHighestPriorityThatCanStart
        {
            get
            {
                foreach (var goal in Goals)
                {
                    var currentlyRunning = goal == CurrentGoal;
                    if (goal.ShouldRun(currentlyRunning))
                    {
                        return goal;
                    }
                }

                return null;
            }
        }
    }
}
