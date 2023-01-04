using UnityEngine;

namespace Combat
{
    public interface IDamagable
    {
        GameObject gameObject { get; }
        public void takeDamage(DamageInfo damage);
    }

}