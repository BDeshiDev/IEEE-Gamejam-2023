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
                    var dirToTarget = d.transform.position - transform.position;
                    if (dirToTarget == Vector3.zero)
                    {
                        dirToTarget = Vector3.up;//handle explosion in player hands where dist = zero
                    }
                    float explosionFactor = dirToTarget.magnitude;
                    dirToTarget /= explosionFactor; //normalize

                    //square falloff based on dist
                    Debug.Log("explosionFactor = " + explosionFactor);
                    explosionFactor /= explosionRadius;
                    Debug.Log("explosionFactor = " + explosionFactor + " " +  ( Mathf.Clamp01(explosionFactor)));
                    explosionFactor =  1- Mathf.Clamp01(explosionFactor);//close = more knockback
                    explosionFactor *= explosionFactor;
                    Debug.Log("explosionFactor = " + explosionFactor);
                    damageForTarget.knockbackMagitude *= explosionFactor;
                    
                    
                    //we want to make the bomb a tool to gain vertical height
                    // it might be that the omb exploes slightly above the player
                    // but in most cases, we want the to move upwards
                    //so we ensure y component is always positive
                    damageForTarget.damageKnockbackDir = dirToTarget;
                    damageForTarget.damageKnockbackDir.y =
                        Mathf.Min(damageForTarget.damageKnockbackDir.y, minVerticalBoost);
                    damageForTarget.damageKnockbackDir = damageForTarget.damageKnockbackDir.normalized;

                    Debug.Log("explosionFactor "  +  explosionFactor + " damageForTarget = " + damageForTarget, d.gameObject);
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
            //explode in player hands
            transform.position = slot.inventory.owner.transform.position;
            explode();
        }

        public override void use2()
        {
            thrower.throwProjectile(slot.inventory.owner);
        }


    }
}