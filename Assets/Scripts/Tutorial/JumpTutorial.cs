using System;
using Core.Input;
using Core.Misc;
using UnityEngine;

namespace Tutorial
{
    public class JumpTutorial: MonoBehaviour
    {
        public GameObject jumpInstructionsText;
        public GameObject jumpHintText;

        private bool playerInRange = false;

        private void Start()
        {
            InputManager.jumpButton.addPerformedCallback(gameObject, handleJumpPressed);
        }

        private void handleJumpPressed()
        {
            if (playerInRange)
            {
                jumpInstructionsText.SetActive(false);
                jumpHintText.SetActive(true);
            }
        }

        public void handlePlayerEntered()
        {
            playerInRange = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(SceneVarTracker.PlayerTag))
            {
                handlePlayerEntered();
            }
        }
    }
}