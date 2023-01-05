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
    }
}