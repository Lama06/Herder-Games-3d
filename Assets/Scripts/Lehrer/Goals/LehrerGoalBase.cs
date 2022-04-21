using HerderGames.AI;

namespace HerderGames.Lehrer.Goals
{
    public abstract class LehrerGoalBase : GoalBase
    {
        public Lehrer Lehrer { get; private set; }

        protected virtual void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }
        public override void EndGoal(GoalEndReason reason)
        {
            base.EndGoal(reason);
            Lehrer.Sprache.SetSatzSource(null);
        }
    }
}