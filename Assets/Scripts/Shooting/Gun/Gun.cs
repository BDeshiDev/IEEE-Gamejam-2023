using Combat.Pickups;
using UnityEngine;

namespace Core.Misc.Core
{
    public abstract class Gun : Item
    {
        public abstract void shoot();
        public override void use1()
        {
            shoot();
            handleOnUse1();
        }

        public override bool canUse1 => true;
        public override bool canUse2 => false;

        public override void use2()
        {
            //we don't have any other thing to do with guns
        }
    }
}