using System.Collections;
using UnityEngine;

namespace HerderGames.AI
{
    [RequireComponent(typeof(AIController))]
    public abstract class GoalBase : MonoBehaviour
    {
        private Coroutine Coroutine;
        
        public abstract bool ShouldRun(bool currentlyRunning);

        public virtual void StartGoal()
        {
            Coroutine = StartCoroutine(Execute());
        }

        public virtual void EndGoal(GoalEndReason reason)
        {
            StopCoroutine(Coroutine);
        }

        public virtual IEnumerator Execute()
        {
            yield return null;
        }
    }
}