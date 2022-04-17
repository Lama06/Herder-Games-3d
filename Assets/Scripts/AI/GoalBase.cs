using System.Collections;
using UnityEngine;

namespace HerderGames.AI
{
    [RequireComponent(typeof(AiController))]
    public abstract class GoalBase : MonoBehaviour
    {
        private Coroutine Coroutine;
        
        public abstract bool ShouldRun(bool currentlyRunning);

        public void StartGoal()
        {
            Coroutine = StartCoroutine(Execute());
            OnStarted();
        }
        
        public virtual void OnStarted()
        {
        }

        public void EndGoal(GoalEndReason reason)
        {
            StopCoroutine(Coroutine);
            OnEnd(reason);
        }

        public virtual void OnEnd(GoalEndReason reason)
        {
        }

        public virtual IEnumerator Execute()
        {
            yield return null;
        }
    }
}