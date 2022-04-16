using UnityEngine;
using UnityEngine.UIElements;

namespace HerderGames.Player
{
    [RequireComponent(typeof(UIDocument))]
    public class InteraktionsMenu : MonoBehaviour
    {
        private int CurrentEintragId;

        private UIDocument UI;

        private void Awake()
        {
            UI = GetComponent<UIDocument>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                SwitchUIDisplayStyle();
            }
        }

        public int AddEintrag(InteractionsMenuEintrag eintrag)
        {
            var id = CurrentEintragId++;
            
            var button = new Button(() => eintrag.Callback(id))
            {
                text = eintrag.Name
            };
            button.AddToClassList("button");
            button.userData = id;
            GetRoot().Add(button);
            return id;
        }

        public void RemoveEintrag(int eintrag)
        {
            var root = GetRoot();
            
            var buttons = root.Query(null, new[] {"button"});
            buttons.ForEach(btn =>
            {
                if ((int) btn.userData == eintrag)
                {
                    root.Remove(btn);
                }
            });
        }

        private VisualElement GetRoot()
        {
            return UI.rootVisualElement.Q("Root");
        }

        private void SwitchUIDisplayStyle()
        {
            switch (GetRoot().style.display.value)
            {
                case DisplayStyle.Flex:
                    GetRoot().style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                    break;
                case DisplayStyle.None:
                    GetRoot().style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                    break;
            }
        }
    }
}