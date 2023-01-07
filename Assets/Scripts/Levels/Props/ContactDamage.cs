using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class ContactDamage : MonoBehaviour
    {
        public DamageInfo damage;

        public Dictionary<IDamagable, float> timeLastDamagedMap =new Dictionary<IDamagable, float>();
        public float minDamageInterval = .8f;
        private void OnTriggerEnter(Collider other)
        {
            var d = other.GetComponent<IDamagable>();
            Debug.Log("contact d = " + d, gameObject);
            if (d != null)
            {
                if (timeLastDamagedMap.TryGetValue(d, out var timeLastDamage))
                {
                    if ((Time.time - timeLastDamage) < minDamageInterval)
                    {
                        return;
                    }
                }

                timeLastDamagedMap[d] = Time.time;
                d.takeDamage(damage);
                
                Debug.Log("contact damage d = " + d + " " + damage, gameObject);

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