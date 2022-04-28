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
        private bool IsFocused;

        private void Awake()
        {
            Document = GetComponent<UIDocument>();
        }

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            var oeffnenBtn = GetInteraktionsMenuOeffnenButton();
            oeffnenBtn.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            oeffnenBtn.clicked += ToggleFocus;
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
            SetElementVisibility(GetInteraktionsMenu(), IsFocused);
#if UNITY_IOS || UNITY_ANDROID
            Cursor.lockState = IsFocused ? CursorLockMode.None : CursorLockMode.Locked;
#endif
        }

        private void UpdateGeld()
        {
            GetGeld().text = $"Geld: {Player.GeldManager.Geld}€";
        }

        private void UpdateZeit()
        {
            var text = new StringBuilder();
            text.Append(TimeManager.GetDisplayText());
            GetZeit().text = text.ToString();
        }

        private void UpdateStundenplan()
        {
            GetStundenplan().text = Player.Stundenplan.GetDisplayText();
        }

        private void UpdateInteraktionsMenu()
        {
            // Es dürfen nicht in jedem Frame alle Buttons entfernt und komplett neu generiert werden,
            // weil sonst das Klicken auf die Buttons nicht funktioniert

            var interaktionsMenu = GetInteraktionsMenu();
            var fehlendeEintraege = new Dictionary<int, InteraktionsMenuEintrag>(Player.InteraktionsMenu.Eintraege);

            foreach (var button in interaktionsMenu.Query<Button>().ToList())
            {
                var id = (int) button.userData;
                if (!Player.InteraktionsMenu.Eintraege.ContainsKey(id))
                {
                    interaktionsMenu.Remove(button);
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
                interaktionsMenu.Add(newButton);
            }
        }

        private void UpdateChat()
        {
            var chat = GetChatWindow();
            chat.Clear();
            foreach (var message in Player.Chat.GetMessages())
            {
                var label = new Label(message)
                {
                    style =
                    {
                        whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal)
                    }
                };
                chat.Add(label);
            }
        }

        public bool GetIsFocused()
        {
            return IsFocused;
        }

        private Label GetGeld()
        {
            return Document.rootVisualElement.Q<Label>("Geld");
        }

        private Label GetZeit()
        {
            return Document.rootVisualElement.Q<Label>("Zeit");
        }

        private Label GetStundenplan()
        {
            return Document.rootVisualElement.Q<Label>("Stundenplan");
        }

        private VisualElement GetInteraktionsMenu()
        {
            return Document.rootVisualElement.Q<VisualElement>("InteraktionsMenu");
        }

        private Button GetInteraktionsMenuOeffnenButton()
        {
            return Document.rootVisualElement.Q<Button>("InteraktionsMenuOeffnen");
        }

        private VisualElement GetChatWindow()
        {
            return Document.rootVisualElement.Q<VisualElement>("Chat");
        }

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
