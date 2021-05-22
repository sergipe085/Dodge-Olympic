using Controllers;
using UnityEngine;

namespace Traps
{
    public class Trap : MonoBehaviour
    {
        public void HitPlayer(PlayerController player, Vector3 impactPoint) {
            StartCoroutine(player.TakeHit(this, impactPoint));
        }
    }
}