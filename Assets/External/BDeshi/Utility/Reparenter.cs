using System;
using UnityEngine;

namespace BDeshi.Utility
{
    public class Reparenter: MonoBehaviour
    {
        public Transform newParent = null;
        private void Awake()
        {
            transform.parent = newParent;
        }
    }
}