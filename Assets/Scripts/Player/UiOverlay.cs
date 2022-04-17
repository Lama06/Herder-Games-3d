using HerderGames.Time;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace HerderGames.Player
{
    public class UiOverlay : MonoBehaviour
    {
        [SerializeField] private UIDocument Document;
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private InteraktionsMenu InteraktionsMenu;
        [SerializeField] private Chat Chat;

        private bool IsFocused;

        public bool GetIsFocused()
        {
            return IsFocused;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleFocus();
            }
            UpdateZeit();
            UpdateInteraktionsMenu();
            UpdateChat();
        }

        private void ToggleFocus()
        {
            IsFocused = !IsFocused;
            SetElementVisibility(GetInteraktionsMenu(), IsFocused);
            Cursor.lockState = IsFocused ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void UpdateZeit()
        {
            GetZeit().text = TimeManager.GetDisplayText();
        }

        private void UpdateInteraktionsMenu()
        {
            var interaktionsMenu = GetInteraktionsMenu();
            interaktionsMenu.Query<Button>().ForEach(btn => interaktionsMenu.Remove(btn));
            foreach (var (eintragId, eintrag) in InteraktionsMenu.Eintraege)
            {
                var button = new Button(() => eintrag.Callback(eintragId))
                {
                    text = eintrag.Name
                };
                interaktionsMenu.Add(button);
            }
        }

        private void UpdateChat()
        {
            var chat = GetChatWindow();
            chat.Clear();
            foreach (var message in Chat.GetMessages())
            {
                var label = new Label(message);
                chat.Add(label);
            }
        }

        private Label GetZeit()
        {
            return Document.rootVisualElement.Q<Label>("Zeit");
        }

        private VisualElement GetInteraktionsMenu()
        {
            return Document.rootVisualElement.Q<VisualElement>("Interaktionsmenu");
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