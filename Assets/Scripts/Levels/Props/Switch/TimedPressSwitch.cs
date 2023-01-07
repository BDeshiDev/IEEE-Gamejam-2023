    using System;
using BDeshi.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Levels.Props.Switch
{
    public class TimedPressSwitch : PressSwitch
    {
        public FiniteTimer deactivateTimer = new FiniteTimer(0, 5f);
        public UnityEvent<float> deactivateProgressEvent;
        protected override void handlePlayerEnterSwitch()
        {
            activate();
        }

        protected override void handlePlayerExitSwitch()
        {
            deactivateTimer.reset();
        }

        private void Update()
        {
            if (!isPlayerOnSwitch)
            {
                if (deactivateTimer.tryCompleteOnce(Time.deltaTime))
                {
                    deactivate();
                }
                else
                {
                    deactivateProgressEvent.Invoke(deactivateTimer.Ratio);
                }
                
            }
        }
    }
}