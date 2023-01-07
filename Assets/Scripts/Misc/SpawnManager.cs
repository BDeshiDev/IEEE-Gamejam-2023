using BDeshi.Utility;
using Combat;
using UnityEngine;

namespace Core.Misc
{
    /// <summary>
    /// Stores and pools prefabs
    /// </summary>
    public class SpawnManager : MonoBehaviourSingletonPersistent<SpawnManager>
    {
        public DamageText gunDamagePrefab;
        public AutoMonobehaviourPool<DamageText> gunDamagePool { get; private set; }
        public PoolableParticleSystem gunShotParticlePrefab;
        public AutoMonobehaviourPool<PoolableParticleSystem> gunShotParticlePool { get; private set; }
        protected override void initialize()
        {
            gunDamagePool = new AutoMonobehaviourPool<DamageText>(gunDamagePrefab,3);
            gunShotParticlePool = new AutoMonobehaviourPool<PoolableParticleSystem>(gunShotParticlePrefab,3);
        }
    }
}