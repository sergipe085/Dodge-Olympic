using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        [Header("FOLLOW PLAYER")]
        [SerializeField]
        private Transform target = null;
        [SerializeField]
        private float            followSpeed     = 10f;
        private Vector3          initialPosition = Vector3.zero;

        [Header("PLAYER JUMP BUG")]
        public bool jumping = false;

        private void Awake() {
            instance = this;

        }

        private void Start() {
            if (target != null) {
                initialPosition = transform.position - target.transform.position;
            }
        }

        private void LateUpdate() {
            FollowTarget(target.transform);
        }

        private void FollowTarget(Transform target) {
            if (target == null) return;
            float deltaTime = jumping ? Time.fixedDeltaTime : Time.deltaTime;
            Vector3 desiredPosition = new Vector3(target.position.x + initialPosition.x, target.position.y + initialPosition.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * deltaTime);
        }
    }
}
