using UnityEngine;

namespace HerderGames.Player
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private float Sensitivity;
        [SerializeField] private Transform PlayerBody;
        [SerializeField] private UiOverlay Overlay;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Overlay.GetIsFocused())
            {
                return;
            }
            
            var mouseX = Input.GetAxis("Mouse X") * Sensitivity * UnityEngine.Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * Sensitivity * UnityEngine.Time.deltaTime;
            
            PlayerBody.Rotate(new Vector3(0, mouseX, 0));
            transform.Rotate(new Vector3(-mouseY, 0, 0));
        }
    }
}