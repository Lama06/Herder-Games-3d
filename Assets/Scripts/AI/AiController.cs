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
            for (var i = 0; i < Goals.Length; i++)
            {
                if (i <= CurrentGoalPriority)
                {
                    continue;
                }

                var goal = Goals[i];

                if (!goal.CanStart())
                {
                    continue;
                }

                return (goal, i);
            }

            return (null, 0);
        }
    }
}