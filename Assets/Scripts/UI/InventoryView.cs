using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Combat.Pickups;
using Core.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class InventoryView: MonoBehaviour
    {
        public Transform inventoryUIContainer;
        public PickupView pickUpViewPrefab;
        public SimpleManualMonoBehaviourPool<PickupView> pickUpViewPool;
        public List<PickupView> activePickupViews;
        public PlayerInventory inventory;
        
        private void Awake()
        {
            pickUpViewPool = new SimpleManualMonoBehaviourPool<PickupView>(
                pickUpViewPrefab, 3, inventoryUIContainer
                );
        }
        
        public void init()
        {
            inventory = SceneVarTracker.Instance.Player.inventory;
            if (inventory != null)
            {
                // as the source of the event is destroyed in scene change,
                // we don't really need to unsub for this  when a new scene is about to be loaded
                inventory.onInventoryRefreshed += refreshUI;
                refreshUI(inventory);
            }
        }

        private void OnDestroy()
        {
            if(inventory != null)
                inventory.onInventoryRefreshed -= refreshUI;
        }

        public void refreshUI(PlayerInventory inventory)
        {
            if (inventory.curPickupIndex  < 0)
            {
                inventoryUIContainer.gameObject.SetActive(false);
                return;
            }
            else
            {
                inventoryUIContainer.gameObject.SetActive(true);
                
            }
            for (int i = 0; i < inventory.obtainedItemSlots.Count; i++)
            {
                var indexBasedOnPickupSelectionOrder = (inventory.curPickupIndex + i + inventory.obtainedItemSlots.Count) % inventory.obtainedItemSlots.Count;
                var pickupInSelectionOrder = inventory.obtainedItemSlots[indexBasedOnPickupSelectionOrder];
                if (i >= activePickupViews.Count)
                {
                    var newView = pickUpViewPool.getItem();
                    activePickupViews.Add(newView);
                }
                activePickupViews[i].refreshUI(pickupInSelectionOrder);
                activePickupViews[i].setIconActiveState(i == 0);
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