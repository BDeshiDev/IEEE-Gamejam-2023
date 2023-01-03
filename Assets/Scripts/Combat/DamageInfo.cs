using System;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class DamageInfo
    {
        public int healthDamage;

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