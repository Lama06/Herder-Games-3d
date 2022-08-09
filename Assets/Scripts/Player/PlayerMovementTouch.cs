using HerderGames.UI;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Player))]
    public class PlayerMovementTouch : MonoBehaviour
    {
        [SerializeField] private float Sensitivity;
        [SerializeField] private float MaxSpeed;

        private Player Player;
        private CharacterController Controller;

        private void Awake()
        {
#if UNITY_ANDROID || UNITY_IOS
            Player = GetComponent<Player>();
            Controller = GetComponent<CharacterController>();
#endif
        }

        private void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Player.VerbrechenManager.BegehtGeradeEinVerbrechen)
            {
                return;
            }

            foreach (var touch in Input.touches)
            {
                if (!(touch.rawPosition.x >= Screen.width / 2))
                {
                    continue;
                }
                
                var x = (touch.position.x - touch.rawPosition.x) * Sensitivity;
                x = Mathf.Clamp(x, -MaxSpeed, MaxSpeed);
                var y = (touch.position.y - touch.rawPosition.y) * Sensitivity;
                y = Mathf.Clamp(y, -MaxSpeed, MaxSpeed);

                var movement = transform.right * x + transform.forward * y;

                Controller.Move(movement * Time.deltaTime);
            }
#endif
        }
    }
}
