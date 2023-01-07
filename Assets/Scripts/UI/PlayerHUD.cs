using System;
using Combat;
using Core.Misc;
using UI.Components;
using UnityEngine;

namespace UI.HUD
{
    public class PlayerHUD: MonoBehaviour
    {
        [SerializeField] private ProgressBar healthViewController;
        [SerializeField] private InventoryView inventoryView;
        private void Start()
        {
            init();
            SceneVarTracker.Instance.onSceneVarsFetched += init;
        }

        private void OnDestroy()
        {
            if (SceneVarTracker.Instance != null)
            {
                SceneVarTracker.Instance.onSceneVarsFetched -= init;
            }
        }



        protected void init()
        {
            var player = SceneVarTracker.Instance.Player;

            if (player != null)
            {
                var healthComponent =  player.HealthComponent;
                healthViewController.init(healthComponent);
            
                inventoryView.init();
            }

        }
        public virtual void enableHUD()
        {
            gameObject.SetActive(true);
            // healthViewController.getGameObject.SetActive(false);
        }
        
        public virtual void disableHUD()
        {
            gameObject.SetActive(false);
            // healthViewController.getGameObject.SetActive(true);
        }
    }
}