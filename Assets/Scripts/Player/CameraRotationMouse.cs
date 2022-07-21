using HerderGames.UI;
using UnityEngine;

namespace HerderGames.Player
{
    public class CameraRotationMouse : MonoBehaviour
    {
        [SerializeField] private float Sensitivity;
        [SerializeField] private Transform PlayerBody;
        [SerializeField] private UIOverlay Overlay;

        private void Start()
        {
#if UNITY_STANDALONE
            Cursor.lockState = CursorLockMode.Locked;
#endif
        }

        private void Update()
        {
#if UNITY_STANDALONE
            if (Overlay.IsFocused)
            {
                return;
            }

            var mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

            PlayerBody.Rotate(new Vector3(0, mouseX, 0));
            transform.Rotate(new Vector3(-mouseY, 0, 0));
#endif
        }
    }
}
