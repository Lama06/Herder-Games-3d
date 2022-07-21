using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class InDerSchuleStatus : MonoBehaviour
    {
        [SerializeField] private bool Status = true;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            InSchule = Status;
        }

        public bool InSchule
        {
            set
            {
                Status = value;

                foreach (var collider in GetComponents<Collider>())
                {
                    collider.enabled = value;
                }

                Lehrer.Renderer.enabled = value;

                // Der NavMeshAgent Component muss deaktiviert werden, weil sonst andere Lehrer nicht zum Ausgang navigieren können,
                // wenn dort bereits ein anderer Lehrer steht, weil die beiden NavMeshAgents nicht kollidiren wollen (obwohl
                // sie unsichtbar sind)
                Lehrer.Agent.enabled = value;

                // Damit der Lehrer nicht redet, während er nicht in der Schule ist
                Lehrer.Sprache.enabled = value;

                Lehrer.FragenManager.enabled = value;
            }
            get => Status;
        }
    }
}
