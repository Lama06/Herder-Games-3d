using System.Collections;
using UnityEngine;

namespace HerderGames.AI
{
    [RequireComponent(typeof(AIController))]
    public abstract class GoalBase : MonoBehaviour
    { 
        public abstract bool ShouldRun(bool currentlyRunning);

        public virtual void StartGoal()
        {
            StartCoroutine(Execute());
        }

        public virtual void EndGoal(GoalEndReason reason)
        {
            StopAllCoroutines();
        }

        public virtual IEnumerator Execute()
        {
            yield return null;
        }
    }
}