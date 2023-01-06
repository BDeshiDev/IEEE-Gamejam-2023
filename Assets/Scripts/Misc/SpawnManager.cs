using BDeshi.Utility;
using Combat;

namespace Core.Misc
{
    /// <summary>
    /// Stores and pools prefabs
    /// </summary>
    public class SpawnManager : MonoBehaviourSingletonPersistent<SpawnManager>
    {
        public DamageText gunDamagePrefab;

        public AutoMonobehaviourPool<DamageText> gunDamagePool { get; private set; }
        protected override void initialize()
        {
            gunDamagePool = new AutoMonobehaviourPool<DamageText>(gunDamagePrefab,3);
        }
    }
}