using System;
using System.Collections.Generic;
using Core.Misc;
using UnityEngine;

namespace Combat
{
    public class NoPassWall: MonoBehaviour
    {
        public DamageInfo damage;

        public Dictionary<IDamagable, float> timeLastDamagedMap =new Dictionary<IDamagable, float>();
        public float minDamageInterval = .8f;
        public float pushBackForce = 8;
        public float pushThroughForce = 20;
        public float pushThroughTeleportOffset;


        public void handleEntry(Transform target)
        {
            var entryDir = transform.position - target.position;
            var dist = entryDir.magnitude;
            entryDir.Normalize();
            
            // if entering from front, always push back, dotprod with damageDir < 0
            // front = side with do not pass text 
            if (Vector2.Dot(entryDir, damage.damageKnockbackDir) < 0)
            {
                Debug.Log("pushback");
                applyKnockback(target, pushBackForce, -entryDir);
            }
            else//If entering from behind, allow pass through
            {
                Debug.Log("pushthrough");
                var cc = target.GetComponent<SimpleCharacterController>();
                if (cc != null)
                {
                    pushThroughTeleportOffset = 1f;
                    cc.teleportTo(transform.position + damage.damageKnockbackDir * pushThroughTeleportOffset);
                }
                applyKnockback(target, pushThroughForce, damage.damageKnockbackDir);
            }
        }

        void applyKnockback(Transform target, float knockBackAmount, Vector3 dir)
        {
            
            var d = target.GetComponent<IDamagable>();
            if (d != null)
            {
                if (timeLastDamagedMap.TryGetValue(d, out var timeLastDamage))
                {
                    if ((Time.time - timeLastDamage) < minDamageInterval)
                    {
                        return;
                    }
                }

                damage.knockbackMagitude = knockBackAmount;
                timeLastDamagedMap[d] = Time.time;
                d.takeDamage(damage);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger  " + other.gameObject);

            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                handleEntry(other.transform);
            }
        }



        //we don't use this because the player could exit and then fall back quickly
        // private void OnTriggerExit(Collider other)
        // {
        //     
        // }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            
            Gizmos.DrawRay(transform.position, damage.damageKnockbackDir * 5);
            Gizmos.DrawSphere(transform.position + damage.damageKnockbackDir * 2 , .25f);
        }
    }
}