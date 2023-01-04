using System;
using UnityEngine;

namespace Levels.Props.Switch
{
    public class SwitchTriggerLocomotor:MonoBehaviour
    {
        public Transform target;

        
        public Vector3 inactivePos;
        public Vector3 activePos;
        


        public void handleSwitchActivated()
        {
            target.position = activePos;
        }
        
        public void handleSwitchDeactivated()
        {
            target.position = inactivePos;
        }

        public void updateState(float switchDeactivateProgress)
        {
            target.position = Vector3.Lerp(activePos, inactivePos, switchDeactivateProgress);
        }
    }
}