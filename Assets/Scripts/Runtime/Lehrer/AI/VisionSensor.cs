using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    public class VisionSensor
    {
        private readonly Lehrer Lehrer;

        public VisionSensor(Lehrer lehrer)
        {
            Lehrer = lehrer;
        }

        public bool CanSee(GameObject other)
        {
            var distance = Vector3.Distance(Lehrer.EyeLocation.position, other.transform.position);

            if (distance > Lehrer.Configuration.MaxViewDistance)
            {
                return false;
            }

            var vectorToOther = other.transform.position - Lehrer.EyeLocation.transform.position;

            var hit = Physics.Raycast(Lehrer.EyeLocation.position, vectorToOther, out var raycastHit, Lehrer.Configuration.MaxViewDistance,
                Physics.AllLayers, QueryTriggerInteraction.Ignore);

            return hit && raycastHit.transform.IsChildOf(other.transform);
        }
    }
}