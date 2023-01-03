using UnityEngine;

namespace Combat.Pickups
{
    public class ItemPickup : Pickup
    {
        public Item item;
        public override void handlePickup(PlayerEntity player)
        {
            Debug.Log("pickup", gameObject);
            player.inventory.addItem(item);
        }

    }
}