using HerderGames.UI;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float Speed;
        [SerializeField] private UIOverlay Overlay;
        [SerializeField] private Player Player;
        
        private CharacterController Controller;

        private void Awake()
        {
            Controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (Overlay.GetIsFocused() || Player.GetVerbrechenManager().BegehtGeradeEinVerbrechen)
            {
                return;
            }
            
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            var movement = transform.right * x + transform.forward * y;

            Controller.Move(movement * UnityEngine.Time.deltaTime * Speed);
        }
    }
}