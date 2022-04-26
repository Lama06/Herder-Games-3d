using System.Collections;
using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    public abstract class GoalBase
    {
        public Lehrer Lehrer { get; private set; }
        private Coroutine ExecuteCoroutine;

        protected GoalBase(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }

        public virtual void OnGoalEnable()
        {
        }

        public abstract bool ShouldRun(bool currentlyRunning);

        public void StartGoal()
        {
            ExecuteCoroutine = Lehrer.StartCoroutine(Execute());
            OnGoalStart();
        }

        public virtual void OnGoalStart()
        {
        }

        public void EndGoal(GoalEndReason reason)
        {
            OnGoalEnd(reason);
            Lehrer.StopCoroutine(ExecuteCoroutine);
            Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            Lehrer.Agent.destination = Lehrer.transform.position;
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
