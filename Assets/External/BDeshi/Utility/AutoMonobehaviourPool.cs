using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BDeshi.Utility
{
    public class AutoMonobehaviourPool<T> where T : MonoBehaviour, AutoPoolable<T>
    {
        private List<T> pool;
        private HashSet<T> loaned = new HashSet<T>();
        
        protected T prefab;
        private Transform spawnParent;
        public bool debug = false;

        public AutoMonobehaviourPool(T prefab, int initialCount, Transform spawnParent = null)
        {
            this.prefab = prefab;
            this.spawnParent = spawnParent;
            pool = new List<T>();
            while (initialCount > 0)
            {
                initialCount--;
                createAndAddToPool();
            }
        }

        T createItem()
        {
            T item = default(T);
                        
            if (spawnParent != null)
            {
                item = Object.Instantiate(this.prefab, spawnParent,false);
            }
            else
            {
                item = Object.Instantiate(this.prefab);
            }


            return item;
        }


        void createAndAddToPool()
        {
            var item = createItem();
            
            item.gameObject.SetActive(false);
            pool.Add(item);
        }

        public T getItem()
        {
            T item = null;
            if (pool.Count > 0)
            {
                item = pool[pool.Count -1];
                pool.RemoveAt(pool.Count - 1);
                item.gameObject.SetActive(true);
            }
            else
            {
                item = createItem();
            }
            item.NormalReturnCallback += handleNormalNormalReturn;
            item.initialize();
            
            loaned.Add(item);
            
            return item;
        }

        public void ensurePoolHasAtleast(int count)
        {
            for (int i = pool.Count; i <= count; i++)
            {
                createAndAddToPool();
            }
        }

        public void returnAll()
        {
            foreach (var item in loaned)
            {
                item.handleForceReturn();
                handleReturnInternal(item);
            }
            
            loaned.Clear();
        }


        void handleNormalNormalReturn(T item)
        {
            handleReturnInternal(item);
            loaned.Remove(item);
        }

        void handleReturnInternal(T item)
        {
            item.transform.parent = spawnParent;
            item.gameObject.SetActive(false);
            item.NormalReturnCallback -= handleNormalNormalReturn;

            pool.Add(item);
        }
    }
    
    public interface AutoPoolable<T>
    {
        /// <summary>
        /// Called after pooled object is taken out of pool
        /// </summary>
        void initialize();
        /// <summary>
        /// Used by pooler to make all pooled objects return to pool
        /// Theo object should do emergency cleanup  
        /// SHOULD NOT INVOKE NormalReturnCallback
        /// </summary>
        void handleForceReturn();
        /// <summary>
        /// The object calls this when the object itself is ready to be pooled back
        /// </summary>
        event Action<T> NormalReturnCallback;
    }
}