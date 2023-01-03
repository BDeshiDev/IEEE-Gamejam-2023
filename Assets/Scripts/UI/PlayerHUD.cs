using System;
using Combat;
using UI.Components;
using UnityEngine;

namespace UI.HUD
{
    public class PlayerHUD: MonoBehaviour
    {
        [SerializeField] private ProgressBar healthViewController;

        private void Start()
        {
            var healthComponent =  GameObject.FindWithTag("Player").GetComponent<PlayerEntity>().HealthComponent;
            Debug.Log("healthComponent = " + healthComponent, healthComponent);
            init(healthComponent);
        }

        protected void init(HealthComponent playerHealthComponent)
        {
            healthViewController.init(playerHealthComponent);
        }
        public virtual void enableHUD()
        {
            gameObject.SetActive(true);
            // healthViewController.gameObject.SetActive(false);
        }
        
        public virtual void disableHUD()
        {
            gameObject.SetActive(false);
            // healthViewController.gameObject.SetActive(true);
        }
    }
}