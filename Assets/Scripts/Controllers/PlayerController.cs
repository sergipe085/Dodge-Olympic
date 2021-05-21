using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers 
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour 
    {   
        [Header("CORE")]
        [SerializeField] private float gravity = 5f;
        private PlayerInput curInput = new PlayerInput();

        [Header("LOCOMOTION")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acelleration = 10f;
        [SerializeField] private float jumpForce = 5f;

        [Header("COMPONENTS")]
        private Rigidbody rig = null;

        #region MonoBehaviour

        private void Awake() {
            rig = GetComponent<Rigidbody>();
        }

        private void Update() {
            CaptureInput();
            HandleInput(curInput);
            ExtraGravity();
        }

        private void FixedUpdate() {
            HandleFixedInput(curInput);
        }

        #endregion

        #region Input

        private PlayerInput CaptureInput() {
            curInput.xMove = Input.GetAxisRaw("Horizontal");
            curInput.jump  = Input.GetButtonDown("Jump");

            return curInput;
        }

        private void HandleInput(PlayerInput input) {
            Jump(input.jump);
        }

        private void HandleFixedInput(PlayerInput input) {
            Move(input.xMove);
        }

        #endregion

        #region Locomotion

        private void Move(float _xMove) {
            Vector3 desiredVelocity = new Vector3(_xMove * speed, rig.velocity.y, 0f);
            rig.velocity = Vector2.Lerp(rig.velocity, desiredVelocity, acelleration * Time.deltaTime);
        }

        private void Jump(bool jump) {
            if (jump) {
                rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void ExtraGravity() {
            rig.AddForce(Vector3.down * gravity * Time.deltaTime);
        }

        #endregion
    }

    public struct PlayerInput {
        public float xMove;
        public bool jump;
    }
}
