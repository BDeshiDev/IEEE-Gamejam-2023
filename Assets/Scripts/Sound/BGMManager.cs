using BDeshi.Utility;
using UnityEngine;

namespace Sound
{
    public class BGMManager: MonoBehaviourSingletonPersistent<BGMManager>
    {

        [SerializeField]AudioSource player;
        protected override void initialize()
        {
            
        }

        public void playBGM()
        {
            // protect my snaity and ears by not playing it in the unity editor
            #if !UNITY_EDITOR
             if (!player.isPlaying)
            {
                player.Play();
            }
            #endif
           
        }

        public void stopPlaying()
        {
            if (player.isPlaying)
            {
                player.Stop();
            }
        }
    }
}