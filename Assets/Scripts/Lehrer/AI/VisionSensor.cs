using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    public class VisionSensor : MonoBehaviour
    {
        [SerializeField] private Transform EyeLocation;
        [SerializeField] private float MaxViewDistance;

        public bool CanSee(GameObject other)
        {
            var distance = Vector3.Distance(EyeLocation.position, other.transform.position);

            if (distance > MaxViewDistance)
            {
                return false;
            }

            var vectorToOther = other.transform.position - transform.position;

            var hit = Physics.Raycast(EyeLocation.position, vectorToOther, out var raycastHit, MaxViewDistance,
                Physics.AllLayers, QueryTriggerInteraction.Ignore);

            return hit && raycastHit.transform.IsChildOf(other.transform);
        }
    }
}