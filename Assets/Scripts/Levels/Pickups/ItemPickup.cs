using UnityEngine;

namespace Combat.Pickups
{
    public class ItemPickup : Pickup
    {
        public Item item;
        public override void handlePickup(PlayerEntity player)
        {
            base.handlePickup(player);
            player.inventory.addItem(item);
            gameObject.SetActive(false);
        }

    }
}