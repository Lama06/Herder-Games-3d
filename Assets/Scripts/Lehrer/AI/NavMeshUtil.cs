using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer.AI
{
    public static class NavMeshUtil
    {
        public static CustomYieldInstruction WaitForNavMeshAgentToArrive(NavMeshAgent agent)
        {
            return new WaitUntil(() => HasReachedDestionation(agent));
        }

        public static bool HasReachedDestionation(NavMeshAgent agent)
        {
            return !agent.pathPending && !agent.hasPath;
        }
    }
}