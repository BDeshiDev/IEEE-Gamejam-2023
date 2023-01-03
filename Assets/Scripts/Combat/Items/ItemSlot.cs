using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Pickups
{
    /// <summary>
    /// Does book keeping for item usage amount
    /// Does not define usage logic
    /// </summary>
    public class ItemSlot: MonoBehaviour
    {
        private Stack<Item> RemainingItems = new Stack<Item>();
        public int itemCount => RemainingItems.Count;
        [FormerlySerializedAs("pickupID")] public string itemID;
        public PlayerInventory inventory;

        public bool shouldShowCountInUI;
        //unity objects do not support constructors so manual initialize function
        public void init(Item item, PlayerInventory inventory)
        {
            this.itemID = item.PickUpID;
            this.shouldShowCountInUI = item.shouldShowAmountInUI;
            this.inventory = inventory;
            
            addItem(item);
        }

        public void addItem(Item item)
        {
            RemainingItems.Push(item);
            item.handleAddedToInventorySlot(this);
        }
        public void handleItemUsage(bool usage1)
        {
            if (RemainingItems.Count > 0)
            {
                var item = RemainingItems.Peek();
                
                if (usage1)
                {
                    if (item.consumesUponUsage1)
                    {
                        RemainingItems.Pop();
                    }
                    item.use1();
                }
                else
                {
                    if (item.consumesUponUsage2)
                    {
                        RemainingItems.Pop();
                    }
                    item.use2();
                }
            }
        }
        
        public void handlePickupUsage1()
        {
            handleItemUsage(true);
        }
        
        public void handlePickupUsage2()
        {
            handleItemUsage(false);
        }
    }
}