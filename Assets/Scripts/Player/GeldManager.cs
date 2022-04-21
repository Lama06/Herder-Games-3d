using UnityEngine;

namespace HerderGames.Player
{
    public class GeldManager : MonoBehaviour
    {
        public int Geld;

        public bool Pay(int amount)
        {
            if (amount > Geld)
            {
                return false;
            }

            Geld -= amount;
            return true;
        }
    }
}