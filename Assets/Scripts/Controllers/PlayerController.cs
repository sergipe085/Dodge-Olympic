using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics;
using Traps;

namespace Controllers 
{
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
        private bool jumping = false;

        [Header("COMPONENTS")]
        private Rigidbody rig       = null;
        private Animator  animator  = null;

        [Header("RAGDOLL")]
        [SerializeField] private Transform hips = null;

        #region MonoBehaviour

        private void Awake() {
            rig       = GetComponent<Rigidbody>();
            animator  = GetComponentInChildren<Animator>();
        }

        private void Update() {
            if (!canInput) return;

            CaptureInput();
            Land();
        }

        private void FixedUpdate() {
            if (!canInput) return;

            HandleFixedInput(curInput);
        }

        void OnTriggerEnter(Collider other) {
            CheckJump(other);
            CheckWin(other);
        }

        #endregion

        #region Core

        private bool IsGrounded() {
            return Physics.Raycast(feet.position, Vector3.down, 0.6f);
        }

        private void CheckJump(Collider other) {
            JumpPoint point = other.GetComponent<JumpPoint>();
            if (point && !jumping) {
                StartCoroutine(JumpTest(transform.position, point.end.position, point.jumpDuration));
            }
        }

        private void CheckWin(Collider other) {
            if (other.CompareTag("Finish")) {
                GameController.instance.GameOver(true);
                DisableInput();

                curInput.xMove = 0;
                rig.velocity = Vector3.zero;
                
                animator.SetBool("run", false);
                animator.SetTrigger("win");
                animator.applyRootMotion = true;
            }
        }

        #endregion

        #region Input

        private PlayerInput CaptureInput() {
            curInput.xMove = -FindObjectOfType<Joystick>().position.x;

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
            
            animator.SetBool("run", _xMove != 0);
        }

        private void Jump(bool jump) {
            rig.velocity = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private IEnumerator JumpTest(Vector3 start, Vector3 end, float duration) {
            Vector3 pos;
            float time = 0f;
            StartJump();

            yield return new WaitForFixedUpdate();

            while(time <= duration) {
                float t = time/duration;
                pos = Vector3.Lerp(start, end, t) + jumpCurve.Evaluate(t) * Vector3.up;
                transform.position = pos;
                time += Time.deltaTime;
                yield return null;
            }

            StopJump();
        }

        private void StartJump() {
            DisableInput();
            CameraController.instance.jumping = true;
            jumping = true;
        }

        private void StopJump() {
            EnableInput();
            CameraController.instance.jumping = false;
            jumping = false;
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

        #endregion
    
        #region TrapInteraction

        public IEnumerator TakeHit(Trap trap, Vector3 impactPoint) {
            StopAllCoroutines();
            StopJump();
            DisableInput();

            rig.isKinematic = true;
            hips.gameObject.SetActive(true);
            animator.enabled = false;
            this.enabled = false;

            Die();
            yield break;
        }

        #endregion
        
        #region Game

        private void Die() {
            GameController.instance.GameOver(false);
        }

        #endregion
    }

    public struct PlayerInput {
        public float xMove;
    }
}
