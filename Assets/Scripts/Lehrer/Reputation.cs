using UnityEngine;

namespace HerderGames.Lehrer
{
    public class Reputation : MonoBehaviour
    {
        public float ReputationsWert { get; set; }
        [SerializeField] private float Misstrauen;
        [SerializeField] private float Gutmuetigkeit;

        public void AddReputation(float amount)
        {
            switch (amount)
            {
                case < 0:
                    amount *= Misstrauen;
                    break;
                case > 0:
                    amount *= Gutmuetigkeit;
                    break;
            }

            ReputationsWert += amount;
        }

        public bool ShouldGoToSchulleitung()
        {
            return ReputationsWert <= -1;
        }

        public void Reset()
        {
            ReputationsWert = 0;
        }
    }
}