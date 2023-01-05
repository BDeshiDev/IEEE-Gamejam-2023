using UnityEngine;

namespace Combat
{
    public interface IDamagable
    {
        GameObject getGameObject();
        Transform getTransform();
        public void takeDamage(DamageInfo damage);
    }

}