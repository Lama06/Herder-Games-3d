using UnityEngine;

namespace HerderGames.Lehrer.AI
{
    [RequireComponent(typeof(Lehrer))]
    public abstract class BrainBase : MonoBehaviour
    {
        protected Lehrer Lehrer { get; private set; }

        private void Awake()
        {
            Lehrer = GetComponent<Lehrer>();
        }

        private void Start()
        {
            RegisterGoals(Lehrer.AI);
        }

        protected abstract void RegisterGoals(AIController ai);
    }
}