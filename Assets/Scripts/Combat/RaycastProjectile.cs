using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Combat
{
    /// <summary>
    /// Projectile that uses raycasts to move
    /// </summary>
    public class RaycastProjectile: MonoBehaviour
    {
        public float collisionRadius = 1;
        
        [SerializeField] private float speed = 5;
        [SerializeField] private float angularSpeed = 0;
        public Vector3 ShotDir;
        public FiniteTimer durationTimer = new FiniteTimer(0,1);
        public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0,1,0,0);
        public LayerMask hitLayer;
        public UnityEvent<IDamagable> onHit;
        public void initialize(Vector3 spawnPos, Vector3 dir)
        {
            transform.position = spawnPos;
            ShotDir = dir;
            transform.allignToDir2D(dir);

            durationTimer.reset();
        }
        

        void Update()
        {
            if (!durationTimer.isComplete)
            {
                durationTimer.updateTimer(Time.deltaTime);
                move(Time.deltaTime);
            }
            else
            {
                handleEnd();
            }
        }


        private bool queryCollision(float checkDistance, out RaycastHit hit)
        {
            return Physics.SphereCast(transform.position,
                collisionRadius,
                (Vector2)ShotDir,
                out hit,
                checkDistance,
                hitLayer
                );
        }

        private void move(float delta)
        {
            var moveAmount = speedCurve.Evaluate(durationTimer.Ratio) *  speed * delta;
            if (queryCollision(moveAmount, out var hit ) && hit.collider != null)
            {
                var d = hit.collider.GetComponent<IDamagable>();
                handleHit(hit.point, d);
                transform.position += ShotDir * hit.distance;
            }
            else
            {
                transform.position += ShotDir * moveAmount;

            }
        }

        protected void handleHit(Vector2 point, IDamagable d)
        {
            onHit.Invoke(d);
            /*var particles = GameplayPoolManager.Instance.particlePool
                .get(hitParticlesPrefab);
            particles.transform.position = point;
            particles.transform.right = -transform.right;*/
            
            handleEnd();
        }

        public void handleEnd()
        {
            Destroy(gameObject);
        }

        public void initialize()
        {
            
        }


    }
}
