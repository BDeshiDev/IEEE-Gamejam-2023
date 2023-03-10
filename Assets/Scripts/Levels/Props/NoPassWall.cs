using System;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Core.Misc;
using UnityEngine;

namespace Combat
{
    public class NoPassWall: MonoBehaviour, IDamagable
    {
        public DamageInfo damage;
        public Transform pushCenter;
        public Dictionary<IDamagable, float> timeLastDamagedMap =new Dictionary<IDamagable, float>();
        public float minDamageInterval = .8f;
        public float pushBackForce = 8;
        public float pushThroughForce = 20;
        public float pushThroughTeleportOffset;

        public Vector3 lastTargetEntryPos;

        public void handleEntry(Transform target, IDamagable d)
        {
            lastTargetEntryPos = target.position;
            
            var entryDir = pushCenter.position- target.position;
            var dist = entryDir.magnitude;
            var entryDirUnnormalized = entryDir;
            entryDir.Normalize();
            
            // if entering from front, always push back, dotprod with damageDir < 0
            // front = side with do not pass text 
            var dotProd = Vector3.Dot(entryDir, getPushThroughDir());
            if (dotProd < 0)
            {
                
                //no pushback allows player to use these as platforms
                // Debug.Log(dotProd + "pushback" + entryDir);
                // applyKnockback(target, pushBackForce, -entryDir);
            }
            else//If entering from behind, allow pass through
            {
                Debug.Log( dotProd + " pushthrough " + entryDir, gameObject);
                
                teleportTargetToOtherSide(target, entryDirUnnormalized);

                applyKnockback(d, pushThroughForce, getPushThroughDir());
            }
        }

        // void handleEntry(Transform target, IDamagable damagable)
        // {
        //     var entryDir = transform.position - target.position;
        //     var entryDirUnnormalized = entryDir;
        //     entryDir.Normalize();
        //     
        //     // if entering from front, always push back, dotprod with damageDir < 0
        //     // front = side with do not pass text 
        //     if (Vector2.Dot(entryDir, getPushThroughDir()) < 0)
        //     {
        //         applyDamageTo(damagable, pushBackForce, -entryDir);
        //     }
        //     else//If entering from behind, allow pass through
        //     {
        //         
        //         teleportTargetToOtherSide(target, entryDirUnnormalized);
        //
        //         applyDamageTo(damagable, pushThroughForce, getPushThroughDir());
        //     }
        // }

        private void teleportTargetToOtherSide(Transform target, Vector3 entryDirUnnormalized)
        {
            var cc = target.GetComponent<SimpleCharacterController>();
            if (cc != null)
            {
                pushThroughTeleportOffset = 1f;
                Vector3 teleportAmount = getPushThroughDir() *
                                         (Vector3.Dot(entryDirUnnormalized, getPushThroughDir()) +
                                          pushThroughTeleportOffset
                                         );
                cc.teleportTo(target.position + teleportAmount);
            }
        }

        void applyKnockback(IDamagable d, float knockBackAmount, Vector3 dir)
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
            Debug.Log("trigger  " + other.gameObject, gameObject);
            var d = other.GetComponent<IDamagable>();
            if (d != null)
            {
                handleEntry(d.getTransform(), d);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                var d = other.GetComponent<IDamagable>();
                if (d != null && timeLastDamagedMap.ContainsKey(d))
                {
                    Debug.Log("d, d.getGameObject = " + d, d.getGameObject());
                    timeLastDamagedMap.Remove(d);
                }
            }
        }


        Vector3 getPushThroughDir()
        {
            return transform.right;
            return getPushThroughDir();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            
            Gizmos.DrawRay(transform.position, getPushThroughDir() * 5);
            Gizmos.DrawSphere(transform.position + getPushThroughDir() * 2 , .25f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastTargetEntryPos, .25f);
            Gizmos.DrawLine(lastTargetEntryPos, pushCenter.position);
        }

        public GameObject getGameObject()
        {
            return gameObject;
        }

        public Transform getTransform()
        {
            return transform;
        }

        public void kill()
        {
            //to destroy the text sibling object we need to destroy the parent
            Destroy(transform.parent.gameObject);
        }
        /// <summary>
        /// The doors are alive. Kill them.
        /// </summary>
        /// <param name="damage"></param>
        public void takeDamage(DamageInfo damage)
        {
            Debug.Log(damage,gameObject);
            if (damage.healthDamage > 0)
            {
                kill();
            }
        }
    }
}