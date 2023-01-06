using Combat;
using Combat.Pickups;
using UnityEngine;

namespace Combat.Pickups
{
    public class HealthPack: Item
    {
        public DamageInfo healDamage= new DamageInfo(100);

        public ProjectileThrower thrower;
        void handleUsage()
        {
            Destroy(gameObject);
        }
        
        public void useOn(IDamagable damagee)
        {
            // -ve damage is healing
            // but we've initialized it to a +ve value
            // so this turns out to just deal normal damage in the end
            damagee.takeDamage(healDamage);
            handleUsage();
        }

        public override void use1()
        {
            thrower.throwProjectile(slot.inventory.owner);
        }



        public override void use2()
        {
            useOn(slot.inventory.owner);
        }


        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            base.handleAddedToInventorySlot(slot);
            this.gameObject.SetActive(false);
        }
    }
}

