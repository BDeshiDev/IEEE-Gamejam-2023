using System;
using BDeshi.Utility;
using Combat;
using UI.Components;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Binds enemy params to world space UI associated with that enemy
    /// </summary>
    public class EnemyUIController: MonoBehaviour
    {
        [SerializeField]private ProgressBar healthBar;
        [SerializeField] private FiniteTimer healthBarShowTimer = new FiniteTimer(1.2f);
        [SerializeField]private GoombaEnemyEntity enemyEntity;

        private void Awake()
        {
            enemyEntity = GetComponentInParent<GoombaEnemyEntity>();
            enemyEntity.TookDamage += handleDamageTaken;
            
            healthBar.gameObject.SetActive(false);
        }

        private void handleDamageTaken(DamageInfo obj)
        {
            healthBarShowTimer.reset();
            healthBar.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (healthBarShowTimer.tryCompleteOnce(Time.deltaTime))
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            healthBar.init(enemyEntity.HealthComponent);
        }
    }
}