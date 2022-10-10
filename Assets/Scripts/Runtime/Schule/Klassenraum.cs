using UnityEngine;

namespace HerderGames.Schule
{
    public class Klassenraum : MonoBehaviour
    {
        [SerializeField] [InspectorName("Name")]
        private string _Name;

        public string Name => _Name;
        public bool PlayerInside { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player.Player>() != null)
            {
                PlayerInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player.Player>() != null)
            {
                PlayerInside = false;
            }
        }
    }
}
