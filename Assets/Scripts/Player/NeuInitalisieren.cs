using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class NeuInitalisieren : MonoBehaviour
    {
        [SerializeField] private PersistentDataManager DataManager;

        private Player Player;

        private void Awake()
        {
            Player = GetComponent<Player>();
        }

        private IEnumerator Start()
        {
            while (true)
            {
                foreach (var interrupted in WaitForLongKeyPress(KeyCode.X, 3f))
                {
                    yield return null;
                    if (interrupted)
                    {
                        goto ContinueOuter;
                    }
                }

                Player.Chat.SendChatMessage("Neu Initalisieren?");

                foreach (var interrupted in WaitForLongKeyPress(KeyCode.X, 3f))
                {
                    yield return null;
                    if (interrupted)
                    {
                        goto ContinueOuter;
                    }
                }

                Player.Chat.SendChatMessage("Sind deine Daten wichtig?");

                foreach (var interrupted in WaitForLongKeyPress(KeyCode.X, 3f))
                {
                    yield return null;
                    if (interrupted)
                    {
                        goto ContinueOuter;
                    }
                }

                Player.Chat.SendChatMessage("Nein!");

                foreach (var interrupted in WaitForLongKeyPress(KeyCode.X, 3f))
                {
                    yield return null;
                    if (interrupted)
                    {
                        goto ContinueOuter;
                    }
                }

                DataManager.DeleteData();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Application.OpenURL("https://www.youtube.com/watch?v=0NXLKOfXkoI");

                ContinueOuter: ;
            }
        }

        private static IEnumerable<bool> WaitForLongKeyPress(KeyCode key, float time)
        {
            var timeSinceStart = 0f;

            while (true)
            {
                if (!Input.GetKey(key))
                {
                    yield return true;
                }

                timeSinceStart += Time.deltaTime;

                if (timeSinceStart >= time)
                {
                    yield break;
                }

                yield return false;
            }
        }
    }
}
