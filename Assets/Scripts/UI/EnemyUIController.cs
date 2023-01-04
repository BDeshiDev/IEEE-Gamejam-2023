using System;
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
        [SerializeField]private GoombaEnemyEntity enemyEntity;

        private void Awake()
        {
            enemyEntity = GetComponentInParent<GoombaEnemyEntity>();
        }

        private void Start()
        {
            healthBar.init(enemyEntity.HealthComponent);
        }
    }
}