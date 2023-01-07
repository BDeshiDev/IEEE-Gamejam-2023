using System;
using System.Collections.Generic;
using Combat.Pickups;
using Core.Misc;
using UnityEngine;

namespace Tutorial
{
    public class HealthPackTutorial: MonoBehaviour
    {
        [SerializeField]private ItemPickup firstHealthPackPickup;
        private Item firstHealthPack;

        public GameObject healthPackDescription;
        public GameObject healthPackEatText;
        public GameObject healthPackChuckText;

        public GameObject extraHealthPackContainer;

        private void Start()
        {
            // these are ordered in the order they would happen in game
            firstHealthPackPickup.onPickedup += handleFirsthealthPackPickedUp;
            
            firstHealthPack = firstHealthPackPickup.item;

        }



        private void handlePlayerEatExpiredHealthpack()
        {
            GameProgressTracker.Instance.hasPlayerDiedFromHealthpack = true;
            Debug.Log("GameProgressTracker.Instance.hasPlayerDiedFromHealthpack = " + GameProgressTracker.Instance.hasPlayerDiedFromHealthpack);
        }

        private void handleFirsthealthPackPickedUp(Pickup obj)
        {
            healthPackDescription.SetActive(false);
            if (!GameProgressTracker.Instance.hasPlayerDiedFromHealthpack)
            {
                // update game progress tracker 
                firstHealthPack.onUse2 += handlePlayerEatExpiredHealthpack;
                
                healthPackEatText.SetActive(true);
            }
            else
            {
                // once the player has thrown the first healthpack
                // spawn more
                firstHealthPack.onUse1 += spawnExtraHealthPickups;

                healthPackChuckText.SetActive(true);
            }
        }


        public void spawnExtraHealthPickups()
        {
            extraHealthPackContainer.SetActive(true);
            healthPackChuckText.SetActive(false);
        }
    }
}