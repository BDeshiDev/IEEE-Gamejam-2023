using UnityEngine;

namespace BDeshi.Utility
{
    public abstract class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; private set; }
        protected bool willGetDestroyedAsDuplicate = false;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
                
                initialize();
            }
            else
            {
                willGetDestroyedAsDuplicate = true;
                Destroy(gameObject);
            }
        }
        

        protected abstract void initialize();
    }

}