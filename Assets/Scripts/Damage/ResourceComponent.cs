using System;
using UnityEngine;

namespace Combat
{
    public class ResourceComponent : MonoBehaviour
    {
        public float Max => max;
        [SerializeField] protected float max = 100;
        public float Cur => cur;
        [SerializeField] protected float cur;

        public float Ratio => ((float)cur) / max;

        public bool IsFull => cur>= max;
        public bool IsEmpty => cur <= 0;

        public event Action<ResourceComponent> Emptied;
        public event Action<ResourceComponent> Capped;
        public event Action<ResourceComponent> RatioChanged;
        

        public void fullyRestore()
        {
            if(cur>=max)
                return;

            cur = max;
            Capped?.Invoke(this);

            RatioChanged?.Invoke(this);
        }

        public void reduceAmount(float amount)
        {
            modifyAmount(-amount);
        }

        public void setRatio(float ratio)
        {
            if (ratio <= 0)
            {
                cur = 0;
                handleEmptied();
            }else if (ratio >= 1)
            {
                cur = max;
                handleCapped();
            }
            else
            {
                cur = ratio * max;
            }
            RatioChanged?.Invoke(this);
        }

        public bool tryUse(float amount)
        {
            if (amount <= cur)
            {
                modifyAmount(-amount);
                return true;
            }

            return false;
        }

        /// <summary>
        /// MOdify resource amount clamped by max and min properly
        /// </summary>
        /// <param name="changeAmount">USE -VE IF IT'S DAMAGE </param>
        public virtual void modifyAmount(float changeAmount)
        {
            if(changeAmount < 0)
            {
                if (cur <= Mathf.Abs(changeAmount))
                {
                    if(cur > 0)
                    {
                        cur = 0;
                        handleEmptied();
                    }
                }
                else
                {
                    cur += changeAmount;
                }
            }
            else
            {
                if (cur < max)
                {
                    cur += changeAmount;
                    if (cur > max)
                    {
                        cur = max;
                        handleCapped();
                    }
                }
            }

            RatioChanged?.Invoke(this);
        }

        public void forceEmpty()
        {
            if(cur >= 0)
            {
                cur = 0;
                handleEmptied();
            }
            RatioChanged?.Invoke(this);
        }

        public virtual void handleEmptied(){
            Emptied?.Invoke(this);
        }
        
        public virtual void handleCapped(){
            Capped?.Invoke(this);
        }
    }
    

}