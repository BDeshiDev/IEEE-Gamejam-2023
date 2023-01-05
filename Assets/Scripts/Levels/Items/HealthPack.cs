using Combat;
using Combat.Pickups;
using UnityEngine;

namespace Combat.Pickups
{
    public class HealthPack: Item
    {
        public int healAmount = -100;

        public ProjectileThrower thrower;
        void handleUsage()
        {
            Destroy(gameObject);
        }
        
        public void useOn(IDamagable damagee)
        {
            // -ve damage is healing
            // but we've initialized it to a -ve value
            // so this turns out to just deal damage in the end
            damagee.takeDamage(new DamageInfo(-healAmount));
            handleUsage();
        }

        public override void use1()
        {
            useOn(slot.inventory.owner);
        }



        public override void use2()
        {
            thrower.throwProjectile(slot.inventory.owner);
        }


        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            base.handleAddedToInventorySlot(slot);
            this.gameObject.SetActive(false);
        }
    }
}

