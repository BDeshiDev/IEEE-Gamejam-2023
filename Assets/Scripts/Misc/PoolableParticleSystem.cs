using System;
using BDeshi.Utility;
using UnityEngine;

namespace Core.Misc
{
    /// <summary>
    /// Make unity particle system work with pooling
    /// Disable looping and set stop action to looping
    /// </summary>
    public class PoolableParticleSystem: MonoBehaviour, AutoPoolable<PoolableParticleSystem>
    {
        public ParticleSystem particleSystem;
        private void OnParticleSystemStopped()
        {
            NormalReturnCallback?.Invoke(this);
        }

        public void initialize()
        {
            if (particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
            particleSystem.Play();
        }

        public void handleForceReturn()
        {
            particleSystem.Stop();
        }

        public event Action<PoolableParticleSystem> NormalReturnCallback;
    }
}