using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace HerderGames.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class TitleScreen : MonoBehaviour
    {
        private UIDocument Document;

        private void Awake()
        {
            Document = GetComponent<UIDocument>();
        }

        private void Start()
        {
#if UNITY_STANDALONE
            Cursor.lockState = CursorLockMode.None;
#endif
            
            StartButton.clicked += () => { SceneManager.LoadScene("Scenes/World"); };
        }

        private Button StartButton => Document.rootVisualElement.Q<Button>("StartGame");
    }
}
