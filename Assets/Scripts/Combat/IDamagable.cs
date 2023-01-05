using UnityEngine;

namespace Combat
{
    public interface IDamagable
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        public void takeDamage(DamageInfo damage);
    }

}