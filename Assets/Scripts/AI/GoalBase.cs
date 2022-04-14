using UnityEngine;

namespace HerderGames.AI
{
    [RequireComponent(typeof(AiController))]
    public abstract class GoalBase : MonoBehaviour
    {
        public abstract bool CanStart();

        public abstract bool ShouldContinue();

        public virtual void OnStarted()
        {
        }

        public virtual void OnTick()
        {
        }

        public virtual void OnEnd(GoalEndReason reason)
        {
        }
    }
}