using System;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(CharacterController))]
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private Transform GroundCheckPosition;
        [SerializeField] private float GroundCheckRadius;
        [SerializeField] private float FallSpeedIncrease;

        private CharacterController CharacterController;
        
        private float FallSpeed;

        private bool IsOnGround => Physics.CheckSphere(GroundCheckPosition.position, GroundCheckRadius);
        
        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (!IsOnGround)
            {
                FallSpeed += FallSpeedIncrease;
                CharacterController.Move(new Vector3(0f, -FallSpeed * Time.deltaTime, 0f));
            }
            else
            {
                FallSpeed = 0f;
            }
        }
    }
}
