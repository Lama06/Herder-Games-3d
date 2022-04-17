using UnityEngine;

namespace HerderGames.AI
{
    public class AiController : MonoBehaviour
    {
        private const int GoalPriorityNoGoal = -1;
        
        private GoalBase[] Goals;
        private int CurrentGoalPriority = GoalPriorityNoGoal;
        private GoalBase CurrentGoal;
        
        private void Awake()
        {
            Goals = GetComponents<GoalBase>();
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
            for (var i = Goals.Length - 1; i >= 0; i--)
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
