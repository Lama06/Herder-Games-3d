using UnityEngine;

namespace HerderGames.Lehrer
{
    public class Reputation : MonoBehaviour
    {
        public float ReputationsWert { get; set; }
        [SerializeField] private float Misstrauen = 1;
        [SerializeField] private float Gutmuetigkeit = 1;

        public void AddReputation(float amount)
        {
            switch (amount)
            {
                case < 0f:
                    amount *= Misstrauen;
                    break;
                case > 0f:
                    amount *= Gutmuetigkeit;
                    break;
            }

            ReputationsWert += amount;
        }

        public bool ShouldGoToSchulleitung()
        {
            return ReputationsWert <= -1f;
        }

        public void ResetAfterMelden()
        {
            ReputationsWert = -0.5f;
        }
    }
}