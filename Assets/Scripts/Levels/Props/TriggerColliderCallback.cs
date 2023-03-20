using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class TriggerColliderCallback:MonoBehaviour
    {
        public UnityEvent onEnter;
        public UnityEvent onExit;
        private void OnTriggerEnter(Collider other)
        {
            onEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            onExit.Invoke();
        }
    }
}