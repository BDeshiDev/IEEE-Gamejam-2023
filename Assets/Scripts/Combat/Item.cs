// using System.Collections.Generic;
// using Combat.Pickups;
// using UnityEngine;
//
// namespace Combat
// {
//     /// <summary>
//     /// Items contain usage logic
//     /// Pickups simply handle how these items get added to inventory
//     /// Items are not tied to actual item count
//     /// As I wanted to have guns that are picked up but do not use  ammo
//     /// </summary>
//     public abstract class Item: MonoBehaviour
//     {
//         protected abstract bool hasRemainingUse();
//         
//          public string ID  {
//              get
//              {
//                  if (pickUpID == null)
//                  {
//                      pickUpID = getPickUpID();
//                  }
//
//                  return pickUpID;
//              }
//          }
//          string pickUpID;
//          private string getPickUpID()
//          {
//              // we want to give each item type a different ID
//              // this returns the derived type
//              // So this works
//              return this.GetType().Name;
//          }
//          
//         /// <summary>
//         /// lmd usage
//         /// </summary>
//         public abstract void use1();
//         /// <summary>
//         /// RMB usage
//         /// </summary>
//         public abstract void use2();
//
//         public abstract void handleDuplicateObtained();
//         public abstract void handleAddedToInventory();
//     }
//     
//     // public class PlayerInventory: MonoBehaviour
//     //  {
//     //      // list rather than Dict because lookup time is irrelevant for only 3 items
//     //      // and we will need to iterate between them in a consistent order
//     //      // which will simply be item order in this case.
//     //      private List<Item> items;
//     //
//     //      public int curPickupIndex= -1;
//     //
//     //      public void addItem(Item item)
//     //      {
//     //          for (int i = 0; i < items.Count; i++)
//     //          {
//     //              if (items[i].ID.Equals(item.ID))
//     //              {
//     //                  items[i].handleDuplicateObtained();
//     //                  return;
//     //              }
//     //          }
//     //         
//     //          var newSlot = getGameObject.AddComponent<ItemSlot>();
//     //          newSlot.amountRemaining = item.useAmountAdded;
//     //          items.Add(newSlot);
//     //      }
//     //      /// <summary>
//     //      /// Will entirely remove item from inventory
//     //      /// Not reduce its usage limit
//     //      /// </summary>
//     //      public void removeItem(Item item)
//     //      {
//     //          for (int i = 0; i < items.Count; i++)
//     //          {
//     //              if (items[i].itemID.Equals(item.PickUpID))
//     //              {
//     //                  items.RemoveAt(i);
//     //                  return;
//     //              }
//     //          }
//     //      }
//     //
//     //      void useCurrentPickup1()
//     //      {
//     //          if (curPickupIndex >= 0 && curPickupIndex <= items.Count )
//     //          {
//     //              handlePickupUsage(string pickUpID)
//     //          }
//     //      }
//     //  }
//
//
//     /// <summary>
//     /// Item that has a limited use count
//     /// Every pick up stacks, increasing the use count to a limit
//     /// </summary>
//     public abstract class StackableItem : Item
//     {
//         [SerializeField] private int count;
//         public int useAmountAddedFromInitialPickup = 1;
//         public int useAmountAddedFromSubsequentPickup = 1;
//         public int maxAmountStackable = 5;
//         public int amountNeededPerUsage1 = 1;
//         public int amountNeededPerUsage2 = 1;
//
//         protected override bool hasRemainingUse()
//         {
//             return count > 0;
//         }
//
//         bool tryUseAmount(int amount)
//         {
//             if (count < amount)
//             {
//                 return false;
//             }
//             count -= amount;
//             return true;
//         }
//
//         public override void use1()
//         {
//             if(tryUseAmount(amountNeededPerUsage1))
//             {
//                 actuallyUse1();
//             }
//         }
//         
//         public override void use2()
//         {
//             if(tryUseAmount(amountNeededPerUsage2))
//             {
//                 actuallyUse2();
//             }
//         }
//
//         public override void handleDuplicateObtained()
//         {
//             count += useAmountAddedFromSubsequentPickup;
//         }
//
//         public override void handleAddedToInventory()
//         {
//             count = useAmountAddedFromInitialPickup;
//         }
//
//         protected abstract void actuallyUse1();
//         protected abstract void actuallyUse2();
//     }
//     
//     public class HealthPack: StackableItem
//     {
//         public int healAmount = -100;
//         [SerializeField]private PlayerEntity playerEntity;
//         /// <summary>
//         /// Apply effect of a healthpack on an IDamagable, not tied to inventory
//         /// </summary>
//         /// <param name="damagee"></param>
//         public void applyOn(IDamagable damagee)
//         {
//             // -ve damage is healing
//             // but we've initialized it to a -ve value
//             // so this turns out to just deal damage in the end
//             damagee.takeDamage(new DamageInfo(-healAmount));
//         }
//
//
//
//         protected override void actuallyUse1()
//         {
//             applyOn(playerEntity);
//         }
//
//         protected override void actuallyUse2()
//         {
//             
//         }
//     }
// }
//
