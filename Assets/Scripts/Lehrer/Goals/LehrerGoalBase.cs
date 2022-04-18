using HerderGames.AI;

namespace HerderGames.Lehrer.Goals
{
    public abstract class LehrerGoalBase : GoalBase
    {
        protected Lehrer Lehrer;

        protected virtual void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }
    }
}