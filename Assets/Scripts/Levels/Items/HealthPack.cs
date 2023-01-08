using Combat;
using Combat.Pickups;
using Core.Misc;
using UnityEngine;

namespace Combat.Pickups
{
    public class HealthPack: Item
    {
        public DamageInfo healDamage= new DamageInfo(100);
        // whether or not this should check hasPlayerDIedFromHealthpack
        // we want to make it so only for the first health in one level
        // so that players can skip to other levels and still use healthpacks
        // without needing to kill themselves once
        public bool checkForPlayerDeath = false;
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

        public override bool canUse2 => true;

        public override bool canUse1 =>
            !checkForPlayerDeath || 
            GameProgressTracker.Instance.hasPlayerDiedFromHealthpack;


        public override void use1()
        {
            thrower.throwProjectile(slot.inventory.owner);
            handleOnUse1();
        }

        public void handleHit(Vector3 point)
        {
            var hitVFX = SpawnManager.Instance.healProjectileHitParticlePool.getItem();
            hitVFX.transform.position = point;
        }



        public override void use2()
        {
            useOn(slot.inventory.owner);
            spawnVFX();

            handleOnUse2();
        }

        private void spawnVFX()
        {
            var healVfxPrefab = SpawnManager.Instance.healParticlePool.getItem();
            healVfxPrefab.transform.parent = slot.inventory.owner.firstPersonParticlesParent;
            healVfxPrefab.transform.localPosition = Vector3.zero;
            healVfxPrefab.transform.localRotation = Quaternion.identity;
        }


        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            base.handleAddedToInventorySlot(slot);
            this.gameObject.SetActive(false);
        }

        public override void handleItemMadeActiveSlot()
        {
            
        }

        public override void handleItemRemovedFromActiveSlot()
        {
            
        }
    }
}

