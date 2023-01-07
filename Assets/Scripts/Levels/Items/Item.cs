using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Pickups
{
    public abstract class Item : MonoBehaviour
    {
        public bool consumesUponUsage1 = true;
        public bool consumesUponUsage2 = true;

        public bool shouldShowAmountInUI;
        public Sprite itemIconSprite;
        [FormerlySerializedAs("ItemCOlor")] public Color ItemColor = Color.green;
        public ItemSlot slot;
        
        /// <summary>
        /// Derived clases should call this manually after use1()
        /// </summary>
        public event Action onUse1;
        /// <summary>
        /// Derived clases should call this manually after use2()
        /// </summary>
        public event Action onUse2;
        
        public abstract bool canUse2 { get; } 
        public abstract bool canUse1 { get; } 

        
        //C# events can't be invoked by derived class
        protected void handleOnUse1()
        {
            onUse1?.Invoke();
        }
        //C# events can't be invoked by derived class
        protected void handleOnUse2()
        {
            onUse2?.Invoke();
        }

        public string PickUpID
        {
            get
            {
                if (pickUpID == null)
                {
                    pickUpID = getPickUpID();
                }

                return pickUpID;
            }
        }

        string pickUpID;

        private string getPickUpID()
        {
            // we want to give each item type a different ID
            // this returns the derived type
            // So this works
            return this.GetType().Name;
        }

        /// <summary>
        /// lmd usage
        /// </summary>
        public abstract void use1();

        /// <summary>
        /// RMB usage
        /// </summary>
        public abstract void use2();

        public virtual void handleAddedToInventorySlot(ItemSlot slot)
        {
            this.slot = slot;
        }
        
        /// <summary>
        /// called when player selects this item as active
        /// </summary>
        public abstract void handleItemMadeActiveSlot();
        
        /// <summary>
        /// called when player  this item is no longer the active item
        /// </summary>
        public abstract void handleItemRemovedFromActiveSlot();
    }
}