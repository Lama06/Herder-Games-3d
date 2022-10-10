using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class Tipps : MonoBehaviour, PersistentDataContainer
    {
        private Player Player;
        
        private readonly List<string> Texte = new()
        {
            "Willkommen in Herder Games!",
            "Du bist in diesem Spiel ein Schüler am Herder Gymnasium und kannst machen, was du willst.",
            "Versuche einen möglichst großen Schaden für die Schule anzurichten und dabei nicht endeckt zu werden.",
            "Wenn du mit einem Lehrer schlecht stehst, wird er eine Verwarnung bei der Schulleitung einreichen. Bei 3 Verwarnungen fliegst du von der Schule.",
            "Du kannst versuchen, dich mit Lehrern gut zu stellen",
            "Wenn du nicht zum Unterricht erscheinst, wird deine Beziehung zum entsprechenden Lehrer schlechter. Du kannst aber versuchen dem Unterricht zu entfliehen."
        };

        private int CurrentTextIndex;

        private void Awake()
        {
            Player = GetComponent<Player>();
        }

        private IEnumerator Start()
        {
            while (true)
            {
                const float MessageDelay = 5f;
                if (CurrentTextIndex > Texte.Count - 1)
                {
                    yield break;
                }

                var text = Texte[CurrentTextIndex];
                CurrentTextIndex++;
            
                Player.Chat.SendChatMessage(text);
            
                yield return new WaitForSeconds(MessageDelay);   
            }
        }

        public void LoadData()
        {
            CurrentTextIndex = PlayerPrefs.GetInt("tips.current_index", 0);
        }

        public void SaveData()
        {
            PlayerPrefs.SetInt("tips.current_index", CurrentTextIndex);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey("tips.current_index");
        }
    }
}
