using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics;

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
        [SerializeField] private float          speed        = 5f;
        [SerializeField] private float          acelleration = 10f;
        [SerializeField] private float          jumpForce    = 5f;
        [SerializeField] private AnimationCurve jumpCurve    = new AnimationCurve();
        private bool land = false;
        private bool canInput = true;

        [Header("COMPONENTS")]
        private Rigidbody rig       = null;

        #region MonoBehaviour

        private void Awake() {
            rig       = GetComponent<Rigidbody>();
        }

        private void Update() {
            if (!canInput) return;

            CaptureInput();
            ExtraGravity();
            Land();
        }

        private void FixedUpdate() {
            if (!canInput) return;

            HandleFixedInput(curInput);
        }

        void OnTriggerEnter(Collider other) {
            CheckJump(other);
        }

        #endregion

        #region Core

        private bool IsGrounded() {
            return Physics.Raycast(feet.position, Vector3.down, 0.6f);
        }

        private void CheckJump(Collider other) {
            JumpPoint point = other.GetComponent<JumpPoint>();
            if (point) {
                StartCoroutine(JumpTest(point, point.jumpDuration));
            }
        }

        #endregion

        #region Input

        private PlayerInput CaptureInput() {
            curInput.xMove = Input.GetAxisRaw("Horizontal");

            return curInput;
        }

        private void HandleFixedInput(PlayerInput input) {
            Move(input.xMove);
        }

        public void DisableInput() {
            canInput = false;
        }

        public void EnableInput() {
            canInput = true;
        }

        #endregion

        #region Locomotion

        private void Move(float _xMove) {
            Vector3 desiredVelocity = new Vector3(_xMove * speed, rig.velocity.y, 0f);
            rig.velocity = Vector2.Lerp(rig.velocity, desiredVelocity, acelleration * Time.deltaTime);

            float rotation = rig.velocity.x > 0 ? 90 : rig.velocity.x == 0 ? transform.eulerAngles.y: -90;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation, 0f), 10f * Time.deltaTime); 
        }

        private void Jump(bool jump) {
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private IEnumerator JumpTest(JumpPoint point, float duration) {
            Vector3 start = transform.position;
            Vector3 end   = point.end.position;
            Vector3 pos;
            float time = 0f;
            DisableInput();
            CameraController.instance.jumping = true;

            yield return new WaitForFixedUpdate();

            while(time <= duration) {
                float t = time/duration;
                pos = Vector3.Lerp(start, end, t) + jumpCurve.Evaluate(t) * Vector3.up;
                transform.position = pos;
                time += Time.fixedDeltaTime;
                yield return null;
            }

            EnableInput();
            CameraController.instance.jumping = false;
        }

        private void Land() {
            if (rig.velocity.y <= -0.2f && IsGrounded() && !land) {
                land = true;
                //land
            }

            if (!IsGrounded()) {
                land = false;
            }
        }

        private void ExtraGravity() {
            rig.AddForce(Vector3.down * gravity * Time.deltaTime);
        }

        #endregion
    }

    public struct PlayerInput {
        public float xMove;
    }
}
