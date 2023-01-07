using System;
using Sound;
using UnityEngine;

namespace Combat.Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        public virtual void handlePickup(PlayerEntity player)
        {
            SFXManager.Instance.play(SFXManager.Instance.pickupSFXPlayer);

        }

        public event Action<Pickup> onPickedup;

        public void handleCollision(GameObject with)
        {
            if (with.CompareTag("Player"))// less expensive check
            {
                var player = with.GetComponent<PlayerEntity>();
                if (player != null)
                {
                    handlePickup(player);
                    onPickedup?.Invoke(this);
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