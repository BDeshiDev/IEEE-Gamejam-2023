using System;
using UnityEngine;

namespace Combat.Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        public abstract void handlePickup(PlayerEntity player);

        public void handleCollision(GameObject with)
        {
            if (with.CompareTag("Player"))// less expensive check
            {
                var player = with.GetComponent<PlayerEntity>();
                if (player != null)
                {
                    handlePickup(player);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            handleCollision(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            handleCollision(collision.gameObject);
        }
    }
}