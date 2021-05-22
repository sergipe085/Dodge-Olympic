using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Traps
{
    public class TrapCollider : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other) {
            Trap trapParent = GetComponentInParent<Trap>();
            PlayerController player = other.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                trapParent.HitPlayer(player, other.contacts[0].point);
            }
        }
    }
}
