using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.AI
{
    public static class NavMeshUtil
    {
        public static CustomYieldInstruction WaitForNavMeshAgentToArrive(NavMeshAgent agent)
        {
            return new WaitUntil(() => !agent.hasPath && !agent.pathPending);
        }
    }
}