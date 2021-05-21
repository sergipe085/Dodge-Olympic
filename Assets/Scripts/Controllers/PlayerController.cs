using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers 
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour 
    {   
        [Header("CORE")]
        [SerializeField] private float     gravity = 5f;
        [SerializeField] private Transform feet    = null;
        private PlayerInput curInput = new PlayerInput();

        [Header("LOCOMOTION")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float acelleration = 10f;
        [SerializeField] private float jumpForce = 5f;
        private bool land = false;

        [Header("COMPONENTS")]
        private Rigidbody rig       = null;
        private ScaleFeel scaleFeel = null;

        #region MonoBehaviour

        private void Awake() {
            rig       = GetComponent<Rigidbody>();
            scaleFeel = GetComponentInChildren<ScaleFeel>();
        }

        private void Update() {
            CaptureInput();
            HandleInput(curInput);
            ExtraGravity();
            Land();
        }

        private void FixedUpdate() {
            HandleFixedInput(curInput);
        }

        #endregion

        #region Core

        private bool IsGrounded() {
            return Physics.Raycast(feet.position, Vector3.down, 0.6f);
        }

        #endregion

        #region Input

        private PlayerInput CaptureInput() {
            curInput.xMove = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump")) {
                StartCoroutine(BetterJump());
            }

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
            if (jump && IsGrounded()) {
                curInput.jump = false;
                rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                scaleFeel?.ChangeScale(new Vector3(-0.6f, 0.6f, -0.6f));
            }
        }

        private void Land() {
            if (rig.velocity.y <= -0.2f && IsGrounded() && !land) {
                land = true;
                scaleFeel?.ChangeScale(new Vector3(0.6f, -0.6f, 0.6f));
            }

            if (!IsGrounded()) {
                land = false;
            }
        }

        private IEnumerator BetterJump() {
            curInput.jump = true;
            yield return new WaitForSeconds(0.8f);
            curInput.jump = false;
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
