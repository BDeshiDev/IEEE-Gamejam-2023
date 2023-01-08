using BDeshi.Utility;
using UnityEngine;

namespace Sound
{
    /// <summary>
    /// just a quick way for gameobjects that destry themselves to play SFX 
    /// </summary>
    public class SFXManager : MonoBehaviourSingletonPersistent<SFXManager>
    {

        public AudioSource pickupSFXPlayer;
        public AudioSource levelCompleteSFX;
        public AudioSource hurtSFX;
        public AudioSource explosionSFX;

        protected override void initialize()
        {
            
        }

        public void play(AudioSource player)
        {
            if (!player.isPlaying)
            {
                player.Play();
            }
        }

        public void stopPlaying(AudioSource player)
        {
            if (player.isPlaying)
            {
                player.Stop();
            }
        }
    }
}