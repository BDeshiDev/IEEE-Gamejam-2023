using UnityEngine;

namespace Combat
{
    public interface IDamagable
    {
        public IDamagable getActualDamageTarget { get; }
        public void takeDamage(DamageInfo damage,ref DamageResult result);
    }

    public enum AttackDodgeResult
    {
        notDodged,
        dodged,
        perfectDodged,
    }

    public enum AttackGuardResult
    {
        Unguarded = 0,
        BadGuard = 1,//hold guard
        GoodGuard = 2,//just not perfect timing
        PerfectGuard = 3,//absolutely perfect timing
    }
    

    public struct DamageResult
    {
        public int dealtDamage;
        public float knockbackMultiplier;
        public float counterStagger;
        public float counterKnockbackMultiplier;
        /// <summary>
        /// The exact point where it hit
        /// </summary>
        public Vector3 hitPoint;
        /// <summary>
        /// This is the direction where the hit sparks should fly off in(technically it's the inverse of this)
        /// </summary>
        public Vector3 hitDir;

        public AttackGuardResult guardResult;
        public AttackDodgeResult dodgeResult;

        public DamageResult(DamageInfo damageInfo, Vector3 hitPoint, Vector3 hitDir) : this()
        {
            this.dealtDamage = damageInfo.healthDamage;
            this.hitPoint = hitPoint;
            this.hitDir = hitDir;
            this.knockbackMultiplier = 1;
            this.counterKnockbackMultiplier = 0;
        }


        public void markAsDodged()
        {
            dealtDamage = 0;
            dodgeResult = AttackDodgeResult.dodged;
            knockbackMultiplier = 0;
        }

        public void markAsGuarded(int postGuardDamage,float knockbackMultiplier,   float counterStagger,float counterKnockbackMultiplier, AttackGuardResult guardResult)
        {
            this.dealtDamage = postGuardDamage;
            this.knockbackMultiplier = knockbackMultiplier;
            this.counterKnockbackMultiplier = counterKnockbackMultiplier;
            this.counterStagger = counterStagger;
            this.guardResult = guardResult;
        }

        public override string ToString()
        {
            return $"dealtDamage:{dealtDamage} guardResult:{guardResult} dodgeResult{dodgeResult} hitPoint{hitPoint} hitDir:{hitDir}";
        }
    }
}