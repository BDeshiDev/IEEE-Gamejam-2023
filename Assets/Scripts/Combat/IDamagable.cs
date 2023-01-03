using UnityEngine;

namespace Combat
{
    public interface IDamagable
    {
        public void takeDamage(DamageInfo damage);
    }

}