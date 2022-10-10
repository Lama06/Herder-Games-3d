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
#if UNITY_STANDALONE
            Cursor.lockState = CursorLockMode.None;
#endif
            
            RestartButton.clicked += () => SceneManager.LoadScene("Scenes/World");
            Schaden.text = $"Schaden für die Schule, der durch dich entstanden ist: {SchadenFuerDieSchule}€";
        }

        private TextElement Schaden => Document.rootVisualElement.Q<TextElement>("Schaden");

        private Button RestartButton => Document.rootVisualElement.Q<Button>("Restart");
    }
}
