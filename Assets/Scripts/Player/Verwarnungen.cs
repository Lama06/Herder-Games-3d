using HerderGames.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class Verwarnungen : MonoBehaviour
    {
        private Player Player;
        
        private int AnzahlVerwarnungen;

        private void Awake()
        {
            Player = GetComponent<Player>();
        }

        public void Add()
        {
            AnzahlVerwarnungen++;
            if (AnzahlVerwarnungen >= 3)
            {
                Debug.Log("Game Over");
                GameOver.SchadenFuerDieSchule = Player.Score.SchadenFuerDieSchule;
                SceneManager.LoadScene("Scenes/GameOver");
            }
        }
    }
}