using System.Collections.Generic;
using System.Text;
using HerderGames.Player;
using HerderGames.Zeit;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace HerderGames.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UIOverlay : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private Player.Player Player;

        private UIDocument Document;
        public bool IsFocused { get; private set; }

        private void Awake()
        {
            Document = GetComponent<UIDocument>();
        }

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            InteraktionsMenuOeffnenButton.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            InteraktionsMenuOeffnenButton.clicked += ToggleFocus;
#endif
        }

        private void Update()
        {
#if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleFocus();
            }
#endif

            UpdateGeld();
            UpdateZeit();
            UpdateStundenplan();
            UpdateInteraktionsMenu();
            UpdateChat();
        }

        private void ToggleFocus()
        {
            IsFocused = !IsFocused;
            SetElementVisibility(InteraktionsMenu, IsFocused);
#if UNITY_STANDALONE
            Cursor.lockState = IsFocused ? CursorLockMode.None : CursorLockMode.Locked;
#endif
        }

        private void UpdateGeld()
        {
            Geld.text = $"Geld: {Player.GeldManager.Geld}€";
        }

        private void UpdateZeit()
        {
            Zeit.text = TimeManager.DisplayText;
        }

        private void UpdateStundenplan()
        {
            Stundenplan.text = Player.Stundenplan.DisplayText;
        }

        private void UpdateInteraktionsMenu()
        {
            // Es dürfen nicht in jedem Frame alle Buttons entfernt und komplett neu generiert werden,
            // weil sonst das Klicken auf die Buttons nicht funktioniert
            
            var fehlendeEintraege = new Dictionary<int, InteraktionsMenuEintrag>(Player.InteraktionsMenu.Eintraege);

            foreach (var button in InteraktionsMenu.Query<Button>().ToList())
            {
                var id = (int) button.userData;
                if (!Player.InteraktionsMenu.Eintraege.ContainsKey(id))
                {
                    InteraktionsMenu.Remove(button);
                    continue;
                }

                fehlendeEintraege.Remove(id);
            }

            foreach (var (id, eintrag) in fehlendeEintraege)
            {
                var newButton = new Button(() => eintrag.Callback(id))
                {
                    userData = id,
                    text = eintrag.Name,
                    focusable = false
                };
                InteraktionsMenu.Add(newButton);
            }
        }

        private void UpdateChat()
        {
            ChatWindow.Clear();
            foreach (var message in Player.Chat.ChatMessages)
            {
                var label = new Label(message)
                {
                    style =
                    {
                        whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal)
                    }
                };
                ChatWindow.Add(label);
            }
        }

        private Label Geld => Document.rootVisualElement.Q<Label>("Geld");

        private Label Zeit => Document.rootVisualElement.Q<Label>("Zeit");

        private Label Stundenplan => Document.rootVisualElement.Q<Label>("Stundenplan");

        private VisualElement InteraktionsMenu => Document.rootVisualElement.Q<VisualElement>("InteraktionsMenu");

        private Button InteraktionsMenuOeffnenButton => Document.rootVisualElement.Q<Button>("InteraktionsMenuOeffnen");

        private VisualElement ChatWindow => Document.rootVisualElement.Q<VisualElement>("Chat");

        private static void SetElementVisibility(VisualElement element, bool visible)
        {
            element.style.display = visible switch
            {
                true => new StyleEnum<DisplayStyle>(DisplayStyle.Flex),
                false => new StyleEnum<DisplayStyle>(DisplayStyle.None)
            };
        }
    }
}
