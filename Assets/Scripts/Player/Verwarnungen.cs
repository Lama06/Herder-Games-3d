using UnityEngine;

namespace HerderGames.Player
{
    public class Verwarnungen : MonoBehaviour
    {
        private int AnzahlVerwarnungen;

        public void Add()
        {
            AnzahlVerwarnungen++;
            if (AnzahlVerwarnungen >= 3)
            {
                Debug.Log("Game Over");
            }
        }

        public int GetAnzahl()
        {
            return AnzahlVerwarnungen;
        }
    }
}