using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat
{
    /// <summary>
    /// Class for defining an instance of damage
    /// Further fields will be added depending on complexity of the project
    /// </summary>
    [Serializable]
    public class DamageInfo
    {
        public int healthDamage;
        public Vector3 damageKnockbackDir;
        public float knockbackMagitude;
        public bool isHeal => healthDamage < 0;
        public DamageInfo(int healthDamage)
        {
            this.healthDamage = healthDamage;
        }

        



        public override string ToString()
        {
            return $"healthDamage: {healthDamage}" ;
        }
    }
}