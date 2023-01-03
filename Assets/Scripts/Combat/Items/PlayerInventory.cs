using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Pickups
{
    public class PlayerInventory: MonoBehaviour
    {
        // list rather than Dict because lookup time is irrelevant for only 3 items
        // and we will need to iterate between them in a consistent order
        // which will simply be item order in this case.
        [FormerlySerializedAs("obtainedItems")] public List<ItemSlot> obtainedItemSlots;

        public int curPickupIndex= -1;
        public PlayerEntity owner;

        public event Action<PlayerInventory> onInventoryRefreshed;
        public void addItem(Item item)
        {
            for (int i = 0; i < obtainedItemSlots.Count; i++)
            {
                if (obtainedItemSlots[i].itemID.Equals(item.PickUpID))
                {
                    obtainedItemSlots[i].addItem(item);
                    onInventoryRefreshed?.Invoke(this);
                    return;
                }
            }

            var newSlot = gameObject.AddComponent<ItemSlot>();
            newSlot.init(item, this);
            obtainedItemSlots.Add(newSlot);
            onInventoryRefreshed?.Invoke(this);
        }

        public void shiftSelectedItem(bool shiftRight)
        {
            if (obtainedItemSlots.Count <= 0)
            {
                return;
            }
            curPickupIndex = (curPickupIndex + (shiftRight ? 1 : -1)) % obtainedItemSlots.Count;
        }
        /// <summary>
        /// Will entirely remove item from inventory
        /// Not reduce its usage limit
        /// </summary>
        public void removeItem(Item item)
        {
            for (int i = 0; i < obtainedItemSlots.Count; i++)
            {
                if (obtainedItemSlots[i].itemID.Equals(item.PickUpID))
                {
                    obtainedItemSlots.RemoveAt(i);
                    break;
                }
            }
            onInventoryRefreshed?.Invoke(this);
        }

        void useCurrentPickup1()
        {
            if (curPickupIndex >= 0 && curPickupIndex <= obtainedItemSlots.Count )
            {
                obtainedItemSlots[curPickupIndex].handlePickupUsage1();
            }
        }
        void useCurrentPickup2()
        {
            if (curPickupIndex >= 0 && curPickupIndex <= obtainedItemSlots.Count )
            {
                obtainedItemSlots[curPickupIndex].handlePickupUsage2();
            }
        }
        
    }
}