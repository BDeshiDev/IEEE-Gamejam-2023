using System;
using BDeshi.Utility;
using UnityEngine;

namespace Core.Misc
{
    public class DamageFlash : MonoBehaviour
    {
        public FiniteTimer flashTimer = new FiniteTimer(.033f);
        public CanvasGroup fader;

        public void doFlash()
        {
            flashTimer.reset();
            gameObject.SetActive(true);
            fader.alpha = 1;
        }

        private void Update()
        {
            if (!flashTimer.isComplete)
            {
                flashTimer.updateTimer(Time.deltaTime);
                fader.alpha = flashTimer.ReverseRatio;
                if (flashTimer.isComplete)
                {
                    stopFlash();
                }
            }
        }

        private void stopFlash()
        {
            flashTimer.complete();
            gameObject.SetActive(false);
        }
    }
}