using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace HerderGames.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class GameOver : MonoBehaviour
    {
        public static int SchadenFuerDieSchule;
        
        private UIDocument Document;

        private void Awake()
        {
            Document = GetComponent<UIDocument>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            
            var restartButton = GetRestartButton();
            restartButton.clicked += () =>
            {
                SceneManager.LoadScene("Scenes/World");
            };

            var schaden = GetSchaden();
            schaden.text = $"Schaden für die Schule, der durch dich entstanden ist: {SchadenFuerDieSchule}";
        }

        private TextElement GetSchaden()
        {
            return Document.rootVisualElement.Q<TextElement>("Schaden");
        }

        private Button GetRestartButton()
        {
            return Document.rootVisualElement.Q<Button>("Restart");
        }
    }
}