using Combat;
using Combat.Pickups;
using UnityEngine;

namespace Combat.Pickups
{
    public class HealthPack: Item
    {
        public int healAmount = -100;
        public RaycastProjectile healthPackProjectile;
        public LayerMask healthPackHitLayer;
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

        public Vector3 getShotDir()
        {
            var ray = slot.inventory.owner.getPlayerShotDirRay();
            if(Physics.Raycast(ray, out var hitResults, Mathf.Infinity, healthPackHitLayer))
            {
                return hitResults.point - slot.inventory.owner.transform.position;
            }
            return slot.inventory.owner.transform.forward;
        }

        public override void use2()
        {
            var proj = Instantiate(healthPackProjectile);
            proj.initialize(slot.inventory.owner.transform.position,getShotDir());
        }


        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            this.slot = slot;
            this.gameObject.SetActive(false);
        }
    }
}

