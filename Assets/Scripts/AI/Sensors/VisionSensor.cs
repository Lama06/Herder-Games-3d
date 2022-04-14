using UnityEngine;

namespace HerderGames.AI.Sensors
{
    public class VisionSensor : MonoBehaviour
    {
        [SerializeField] private Transform EyeLocation;
        [SerializeField] private float MaxViewDistance;
        [SerializeField] private float FieldOfView;

        public bool CanSee(GameObject other)
        {
            var vectorToOther = other.transform.position - transform.position;

            if (vectorToOther.magnitude > MaxViewDistance)
            {
                return false;
            }

            if (Physics.Raycast(EyeLocation.position, vectorToOther, out var raycastHit, MaxViewDistance, Physics.AllLayers,
                    QueryTriggerInteraction.Ignore))
            {
                if (!raycastHit.transform.IsChildOf(other.transform))
                {
                    return false;
                }
            }

            return true;
        }
    }
}