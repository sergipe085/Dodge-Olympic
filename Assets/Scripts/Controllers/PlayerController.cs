using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers 
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour 
    {   
        [Header("CORE")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = 5f;
        private PlayerInput curInput = new PlayerInput();

        [Header("COMPONENTS")]
        private Rigidbody rig = null;

        #region MonoBehaviour

        private void Awake() {
            rig = GetComponent<Rigidbody>();
        }

        private void Update() {
            HandleInput(CaptureInput());
            ExtraGravity();
        }

        #endregion

        #region Input

        private PlayerInput CaptureInput() {
            curInput.xMove = Input.GetAxisRaw("Horizontal");
            curInput.jump  = Input.GetButtonDown("Jump");

            return curInput;
        }

        private void HandleInput(PlayerInput input) {
            Move(input.xMove);
            Jump(input.jump);
        }

        #endregion

        #region Locomotion

        private void Move(float _xMove) {
            Vector3 desiredVelocity = new Vector3(_xMove * speed, rig.velocity.y, 0f);
            rig.velocity = Vector2.Lerp(rig.velocity, desiredVelocity, 5f * Time.deltaTime);
        }

        private void Jump(bool jump) {
            if (jump) {
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void ExtraGravity() {
            rig.AddForce(Vector3.down * gravity);
        }

        #endregion
    }

    public struct PlayerInput {
        public float xMove;
        public bool jump;
    }
}
