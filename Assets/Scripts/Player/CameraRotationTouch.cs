using HerderGames.UI;
using UnityEngine;

namespace HerderGames.Player
{
    public class CameraRotationTouch : MonoBehaviour
    {
        [SerializeField] private float Sensitivity;
        [SerializeField] private Transform PlayerBody;
        [SerializeField] private UIOverlay Overlay;
        
        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Overlay.GetIsFocused())
            {
                return;
            }
            
            foreach (var touch in Input.touches)
            {
                if (!(touch.rawPosition.x <= Screen.width / 2))
                {
                    continue;
                }

                var mouseX = touch.deltaPosition.x * Sensitivity * Time.deltaTime;
                var mouseY = touch.deltaPosition.y * Sensitivity * Time.deltaTime;

                PlayerBody.Rotate(new Vector3(0, mouseX, 0));
                transform.Rotate(new Vector3(-mouseY, 0, 0));
            }
#endif
        }
    }
}
