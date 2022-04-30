using HerderGames.UI;
using HerderGames.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class Verwarnungen : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private PersistentDataManager PersistentDataManager;
        
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
                OnGameOver();
            }
        }

        private void OnGameOver()
        {
            Debug.Log("Game Over");
            GameOver.SchadenFuerDieSchule = Player.Score.SchadenFuerDieSchule;
            PersistentDataManager.Reset();
            SceneManager.LoadScene("Scenes/GameOver");
        }

        private const string SaveKey = "player.verwarnungen";
        
        public void SaveData()
        {
            PlayerPrefs.SetInt(SaveKey, AnzahlVerwarnungen);
        }

        public void LoadData()
        {
            AnzahlVerwarnungen = PlayerPrefs.GetInt(SaveKey);
        }

        public void ResetData()
        {
            AnzahlVerwarnungen = 0;
        }
    }
}