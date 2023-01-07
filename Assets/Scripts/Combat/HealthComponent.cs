using UnityEngine;

namespace Combat
{
    public class HealthComponent : ResourceComponent
    {
        [SerializeField] bool shouldFullyRestore = true;
        private void Start()
        {
            if (shouldFullyRestore)
            {
                fullyRestore();
                
            }
        }
    }
}