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
        }

        public override void use2()
        {
            
        }
    }
}