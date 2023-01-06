using System;
using BDeshi.Utility;
using Core.Misc;
using TMPro;
using UnityEngine;

namespace Combat
{
    public class DamageText: MonoBehaviour, AutoPoolable<DamageText>
    {
        public TextMeshPro text;

        public float moveUpBaseSpeed = 10;
        public AnimationCurve moveUpCurve;
        public FiniteTimer durationTimer = new FiniteTimer(1);
        
        private void Update()
        {
            durationTimer.updateTimer(Time.deltaTime);
            if (durationTimer.isComplete)
            {
                NormalReturnCallback?.Invoke(this);
            }
            else
            {
                transform.position += moveUpBaseSpeed * moveUpCurve.Evaluate(durationTimer.Ratio) * Time.deltaTime * Vector3.up;
                text.color = new Color(text.color.r, text.color.g ,text.color.b,1 - durationTimer.Ratio) ;
            }
            
            transform.forward = SceneVarTracker.Instance.Camera.transform.forward;
        }

        public void initialize()
        {
            durationTimer.reset();
            text.color = new Color(text.color.r, text.color.g ,text.color.b,1) ;
        }

        public void handleForceReturn()
        {
            
        }

        public event Action<DamageText> NormalReturnCallback;
    }
}