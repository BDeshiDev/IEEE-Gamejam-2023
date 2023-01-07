using System;
using BDeshi.Utility;
using Core.Misc;
using UnityEngine;

namespace Combat.Pickups
{
    public class Bomb : Item
    {
        public float explosionRadius = 5;
        public LayerMask damageMask;
        public DamageInfo explosionDamage;
        [SerializeField]private bool active = false;
        public FiniteTimer activeTimer = new FiniteTimer(3);
        public float minVerticalBoost = 5;

        private bool hasExploded = false;
        public ProjectileThrower thrower;
        private void Update()
        {
            if (active)
            {
                if (activeTimer.tryCompleteOnce(Time.deltaTime))
                {
                    explode();
                }
            }
        }

        public void explode()
        {
            if (!hasExploded)
            {
                damageTargetsInRange();
                hasExploded = true;
            }

        }

        private void damageTargetsInRange()
        {
            var targets =
                Physics.OverlapSphere(transform.position, explosionRadius, damageMask, QueryTriggerInteraction.Collide);

            foreach (var target in targets)
            {
                var d = target.GetComponent<IDamagable>();

                if (d != null)
                {
                    var damageForTarget = explosionDamage;
                    var dirToTarget = d.getTransform().position - transform.position;
                    if (dirToTarget == Vector3.zero)
                    {
                        dirToTarget = Vector3.up;//handle explosion in player hands where dist = zero
                    }
                    float explosionFactor = dirToTarget.magnitude;
                    dirToTarget /= explosionFactor; //normalize

                    //square falloff based on dist
                    explosionFactor /= explosionRadius;
                    explosionFactor =  1- Mathf.Clamp01(explosionFactor);//close = more knockback
                    explosionFactor *= explosionFactor;
                    damageForTarget.knockbackMagitude *= explosionFactor;
                    
                    
                    //we want to make the bomb a tool to gain vertical height
                    // it might be that the omb exploes slightly above the player
                    // but in most cases, we want the to move upwards
                    //so we ensure y component is always positive
                    damageForTarget.damageKnockbackDir = dirToTarget;
                    damageForTarget.damageKnockbackDir.y =
                        Mathf.Min(damageForTarget.damageKnockbackDir.y, minVerticalBoost);
                    damageForTarget.damageKnockbackDir = damageForTarget.damageKnockbackDir.normalized;

                    d.takeDamage(damageForTarget);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        public override bool canUse2 => false;
        public override bool canUse1 => true;

        public override void use1()
        {
            spawnBombParticles();
            
            //explode in player hands
            transform.position = slot.inventory.owner.transform.position;
            explode();
            
            handleOnUse2();
        }

        private void spawnBombParticles()
        {
            var bombVFX = SpawnManager.Instance.bombParticlePool.getItem();
            bombVFX.transform.position = slot.inventory.owner.bombSpawnParent.position;
        }

        //there are no levels in game where we actually want this
        public override void use2()
        {
            // thrower.throwProjectile(slot.inventory.owner);
        }

        public override void handleItemMadeActiveSlot()
        {
            
        }

        public override void handleItemRemovedFromActiveSlot()
        {
            
        }
    }
}