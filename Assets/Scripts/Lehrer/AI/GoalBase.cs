using System.Collections;
using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    [RequireComponent(typeof(AIController))]
    public abstract class GoalBase : MonoBehaviour
    {
        public Lehrer Lehrer { get; private set; }

        private Coroutine ExecuteCoroutine;
        
        protected virtual void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        public abstract bool ShouldRun(bool currentlyRunning);

        public void StartGoal()
        {
            ExecuteCoroutine = StartCoroutine(Execute());
            OnGoalStart();
        }

        public virtual void OnGoalStart()
        {
        }

        public void EndGoal(GoalEndReason reason)
        {
            OnGoalEnd(reason);
            StopCoroutine(ExecuteCoroutine);
            Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            Lehrer.Agent.destination = transform.position;
        }

        public virtual void OnGoalEnd(GoalEndReason reason)
        {
        }

        public virtual IEnumerator Execute()
        {
            yield return null;
        }
    }
}