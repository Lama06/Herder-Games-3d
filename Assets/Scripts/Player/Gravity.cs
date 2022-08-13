using System;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(CharacterController))]
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private float FallSpeedIncrease;
        private CharacterController CharacterController;
        private float FallSpeed;

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!CharacterController.isGrounded)
            {
                FallSpeed += FallSpeedIncrease * Time.deltaTime;
                CharacterController.Move(new Vector3(0f, -FallSpeed * Time.deltaTime, 0f));
            }
            else
            {
                FallSpeed = 0f;
            }
        }
    }
}
