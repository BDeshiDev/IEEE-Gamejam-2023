using System;
using Core.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Levels.Props.Switch
{
    public abstract class PressSwitch: MonoBehaviour
    {
        public Vector3 pressedStatePos;
        
        public Vector3 unpressedStatePos;
        
        [SerializeField]protected bool isActive;
        [SerializeField]protected bool isPlayerOnSwitch;
        public bool isSwitchActive => isActive;
        public Transform pressablePortion;
        
        public UnityEvent activatedEvent;
        public UnityEvent deactivatedEvent;
        
        public void activate()
        {
            if(!isActive)
            {
                isActive = true;
                pressablePortion.localPosition = pressedStatePos;
                activatedEvent.Invoke();
            }
        }
        
        public void deactivate()
        {
            if (isActive)
            {
                isActive = false;
                pressablePortion.localPosition = unpressedStatePos;
                deactivatedEvent.Invoke();
            }
        }
        
        /// <summary>
        /// For projectiles, if proj dir and activate dir angle is <= 90  degree then it can activate the switches
        /// </summary>
        /// <returns></returns>
        public Vector3 getPressDirection()
        {
            return -transform.up;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                Debug.Log("player enter");

                isPlayerOnSwitch = true;
                handlePlayerEnterSwitch();
            }
        }

        protected abstract void handlePlayerEnterSwitch();

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                Debug.Log("player exit");
                isPlayerOnSwitch = false;
                handlePlayerExitSwitch();
            }
        }

        protected abstract void handlePlayerExitSwitch();
    }
}