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
    public struct DamageInfo
    {
        public int healthDamage;
        public Vector3 damageKnockbackDir;
        public float knockbackMagitude;
        public bool resetJump;
        public bool resetGravity;

        public DamageCategory damageType;
        
        public bool isHeal => healthDamage < 0;
        public DamageInfo(int healthDamage)
        {
            this.healthDamage = healthDamage;
            this.damageKnockbackDir = Vector3.zero;
            this.knockbackMagitude = 0;
            this.resetJump = false;
            this.resetGravity = false;
            damageType = DamageCategory.Shot;
        }

        



        public override string ToString()
        {
            return $"healthDamage: {healthDamage} knockbackMagitude: {knockbackMagitude} damageKnockbackDir: {damageKnockbackDir}" ;
        }
    }
    public enum DamageCategory
    {
            Shot,
            Explosive//we want to use bombs to repair walls, we don't want gunshots to be able to do that so diff damagetype
    }
}