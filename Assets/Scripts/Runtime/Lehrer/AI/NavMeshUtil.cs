using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer.AI
{
    public static class NavMeshUtil
    {
        public static IEnumerable Pathfind(NavMeshAgent agent)
        {
            while (!HasReachedDestionation(agent))
            {
                yield return null;
            }
        }

        public static bool HasReachedDestionation(NavMeshAgent agent)
        {
            return !agent.pathPending && !agent.hasPath;
        }
    }
}