using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Combat.Pickups;
using UnityEngine;

namespace UI
{
    public class InventoryView: MonoBehaviour
    {
        public Transform inventoryUIContainer;
        public PickupView pickUpViewPrefab;
        public SimpleManualMonoBehaviourPool<PickupView> pickUpViewPool;
        public List<PickupView> activePickupViews;
        public PlayerInventory inventory;
        
        private void Start()
        {
            pickUpViewPool = new SimpleManualMonoBehaviourPool<PickupView>(
                pickUpViewPrefab, 3, inventoryUIContainer
                );

            inventory = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerInventory>();

            inventory.onInventoryRefreshed += refreshUI;
        }

        private void OnDestroy()
        {
            if(inventory != null)
                inventory.onInventoryRefreshed -= refreshUI;
        }

        public void refreshUI(PlayerInventory inventory)
        {
            
            for (int i = 0; i < inventory.obtainedItemSlots.Count; i++)
            {
                var indexBasedOnPickupSelectionOrder = (inventory.curPickupIndex + i) % inventory.obtainedItemSlots.Count;
                var pickupInSelectionOrder = inventory.obtainedItemSlots[indexBasedOnPickupSelectionOrder];
                if (i >= activePickupViews.Count)
                {
                    var newView = pickUpViewPool.getItem();
                    activePickupViews.Add(newView);
                }
                activePickupViews[i].refreshUI(pickupInSelectionOrder);
            }
            //reverse order loop to remove extra activePickupViews
            for (int i =  activePickupViews.Count -1; i >= inventory.obtainedItemSlots.Count; i--)
            {
                var removed = activePickupViews[i];
                activePickupViews.RemoveAt(i);
                pickUpViewPool.returnItem(removed);
            }
        }
        
        
    }
}