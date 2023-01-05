using System;
using BDeshi.Utility;
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
                    Debug.Log("d = " + d);
                    var damageForTarget = explosionDamage;
                    var dirToTarget = d.transform.position - transform.position;
                    var explosionFactor = dirToTarget.magnitude;
                    dirToTarget /= explosionFactor; //normalize

                    //square falloff based on dist
                    explosionFactor /= explosionRadius;
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

        public override void use1()
        {
            
        }

        public override void use2()
        {
            
        }

        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            
        }
    }
}