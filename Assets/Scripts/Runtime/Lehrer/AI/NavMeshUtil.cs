using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace HerderGames.Lehrer.AI
{
    public static class NavMeshUtil
    {
        public static IEnumerable Pathfind(Lehrer lehrer, Transform transform)
        {
            return Pathfind(lehrer, transform.position, transform.rotation);
        }
        
        public static IEnumerable Pathfind(Lehrer lehrer, Vector3 position, Quaternion? rotation = null)
        {
            lehrer.Agent.enabled = true;
            lehrer.Agent.destination = position;
            
            while (!HasReachedDestionation(lehrer.Agent))
            {
                yield return null;
            }

            lehrer.Agent.ResetPath();
            lehrer.Agent.enabled = false;
            
            lehrer.transform.position = position;
            if (rotation != null)
            {
                lehrer.gameObject.transform.rotation = (Quaternion) rotation;
            }
        }

        private static bool HasReachedDestionation(NavMeshAgent agent)
        {
            return !agent.pathPending && !agent.hasPath;
        }
    }
}