using UnityEngine;

namespace Combat.Pickups
{
    public class HealthPack: MonoBehaviour
    {
        public int healAmount = -100;

        public void handleUsage()
        {
            Destroy(gameObject);
        }

        public void useHealthPack(IDamagable damagee)
        {
            // -ve damage is healing
            // but we've initialized it to a -ve value
            // so this turns out to just deal damage in the end
            damagee.takeDamage(new DamageInfo(-healAmount));
            
            handleUsage();
        } 
    }
}