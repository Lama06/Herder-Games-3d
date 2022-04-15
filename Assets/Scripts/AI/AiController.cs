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
            SelectNewGoal();
            if (CurrentGoal != null)
            {
                CurrentGoal.OnTick();
            }
        }

        private void SelectNewGoal()
        {
            if (CurrentGoal != null && !CurrentGoal.ShouldContinue())
            {
                CurrentGoal.OnEnd(GoalEndReason.GoalCannotContinue);
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
                CurrentGoal.OnEnd(GoalEndReason.OtherGoalWithHigherPriority);
            }

            CurrentGoal = newGoal;
            CurrentGoal.OnStarted();
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

                if (!goal.CanStart())
                {
                    continue;
                }

                return (goal, priority);
            }

            return (null, GoalPriorityNoGoal);
        }
    }
}
