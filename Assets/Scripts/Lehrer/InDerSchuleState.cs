using UnityEngine;

namespace HerderGames.Lehrer
{
    [RequireComponent(typeof(Lehrer))]
    public class InDerSchuleState : MonoBehaviour
    {
        [SerializeField] private bool Status = true;

        private Lehrer Lehrer;

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            SetInSchule(Status);
        }

        public void SetInSchule(bool inSchule)
        {
            Status = inSchule;
            
            foreach (var collider in GetComponents<Collider>())
            {
                if (collider.enabled != inSchule)
                {
                    collider.enabled = inSchule;
                }
            }
            
            if (Lehrer.Renderer.enabled != inSchule)
            {
                Lehrer.Renderer.enabled = inSchule;   
            }
            
            // Der NavMeshAgent Component muss deaktiviert werden, weil sonst andere Lehrer nicht zum Ausgang navigieren können,
            // wenn dort bereits ein anderer Lehrer steht, weil die beiden NavMeshAgents nicht kollidiren wollen (obwohl
            // sie unsichtbar sind)
            if (Lehrer.Agent.enabled != inSchule)
            {
                Lehrer.Agent.enabled = inSchule;   
            }

            // Damit der Lehrer nicht redet, während er nicht in der Schule ist
            if (!inSchule)
            {
                Lehrer.Sprache.SaetzeMoeglichkeiten = null;
            }
        }

        public bool GetInSchule()
        {
            return Status;
        }
    }
}