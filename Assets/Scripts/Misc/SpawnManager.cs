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
        [SerializeField] DamageText gunDamagePrefab;
        public AutoMonobehaviourPool<DamageText> gunDamagePool { get; private set; }
        [SerializeField]PoolableParticleSystem gunShotParticlePrefab;
        [SerializeField]PoolableParticleSystem healParticlePrefab;
        [SerializeField]PoolableParticleSystem healProjectileHitParticlePrefab;
        [SerializeField]PoolableParticleSystem bombParticlePrefab;
        public AutoMonobehaviourPool<PoolableParticleSystem> gunShotParticlePool { get; private set; }
        public AutoMonobehaviourPool<PoolableParticleSystem> healParticlePool { get; private set; }
        public AutoMonobehaviourPool<PoolableParticleSystem> healProjectileHitParticlePool { get; private set; }
        public AutoMonobehaviourPool<PoolableParticleSystem> bombParticlePool { get; private set; }
        protected override void initialize()
        {
            // alot of levels don't have any of these so initialize to 0
            gunDamagePool = new AutoMonobehaviourPool<DamageText>(gunDamagePrefab,0,transform);
            gunShotParticlePool = new AutoMonobehaviourPool<PoolableParticleSystem>(gunShotParticlePrefab,0,transform);
            healParticlePool = new AutoMonobehaviourPool<PoolableParticleSystem>(healParticlePrefab,0,transform);
            healProjectileHitParticlePool = new AutoMonobehaviourPool<PoolableParticleSystem>(healProjectileHitParticlePrefab,0,transform);
            bombParticlePool = new AutoMonobehaviourPool<PoolableParticleSystem>(bombParticlePrefab,0,transform);
        }
    }
}