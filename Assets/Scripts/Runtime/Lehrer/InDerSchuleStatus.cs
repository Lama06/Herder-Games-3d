using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class InDerSchuleStatus : MonoBehaviour
    {
        private bool Status;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            InSchule = true;
        }

        public bool InSchule
        {
            set
            {
                Status = value;

                foreach (var collider in GetComponentsInChildren<Collider>())
                {
                    collider.enabled = value;
                }

                foreach (var renderer in Lehrer.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = value;
                }

                // Damit der Lehrer nicht redet, während er nicht in der Schule ist
                Lehrer.Sprache.enabled = value;

                Lehrer.FragenManager.enabled = value;
            }
            get => Status;
        }
    }
}
