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
            var entryDirUnnormalized = entryDir;
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
                
                teleportTargetToOtherSide(target, entryDirUnnormalized);

                applyKnockback(target, pushThroughForce, damage.damageKnockbackDir);
            }
        }

        void handleEntry(Transform target, IDamagable damagable)
        {
            var entryDir = transform.position - target.position;
            var entryDirUnnormalized = entryDir;
            entryDir.Normalize();
            
            // if entering from front, always push back, dotprod with damageDir < 0
            // front = side with do not pass text 
            if (Vector2.Dot(entryDir, damage.damageKnockbackDir) < 0)
            {
                applyDamageTo(damagable, pushBackForce, -entryDir);
            }
            else//If entering from behind, allow pass through
            {
                
                teleportTargetToOtherSide(target, entryDirUnnormalized);

                applyDamageTo(damagable, pushThroughForce, damage.damageKnockbackDir);
            }
        }

        private void teleportTargetToOtherSide(Transform target, Vector3 entryDirUnnormalized)
        {
            var cc = target.GetComponent<SimpleCharacterController>();
            if (cc != null)
            {
                pushThroughTeleportOffset = 1f;
                Vector3 teleportAmount = damage.damageKnockbackDir *
                                         (Vector3.Dot(entryDirUnnormalized, damage.damageKnockbackDir) +
                                          pushThroughTeleportOffset
                                         );
                cc.teleportTo(transform.position + teleportAmount);
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

                applyDamageTo(d, knockBackAmount,  dir);
            }
        }
        
        /// <summary>
        /// deadl damage and update map
        /// </summary>
        /// <param name="d"></param>
        /// <param name="knockBackAmount"></param>
        /// <param name="dir"></param>
        private void applyDamageTo(IDamagable d, float knockBackAmount,  Vector3 dir)
        {
            //preserve the damage fields values by applying changes to a copy by value
            var damageToApply = damage;
            damageToApply.damageKnockbackDir = dir;
            damageToApply.knockbackMagitude = knockBackAmount;
            
            timeLastDamagedMap[d] = Time.time;
            d.takeDamage(damageToApply);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger  " + other.gameObject);

            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                handleEntry(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                var d = other.GetComponent<IDamagable>();
                if (d != null && timeLastDamagedMap.ContainsKey(d))
                {
                    Debug.Log("d, d.gameObject = " + d, d.gameObject);
                    timeLastDamagedMap.Remove(d);
                }
            }
        }

    

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            
            Gizmos.DrawRay(transform.position, damage.damageKnockbackDir * 5);
            Gizmos.DrawSphere(transform.position + damage.damageKnockbackDir * 2 , .25f);
        }
    }
}