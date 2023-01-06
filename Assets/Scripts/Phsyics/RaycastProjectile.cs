using System;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
        [SerializeField] private bool affectedByGravity;
        [SerializeField] private float gravity = 27.77778f;
        public Vector3 ShotDir;
        public FiniteTimer durationTimer = new FiniteTimer(0,1);
        public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0,1,0,0);
        public LayerMask hitLayer;
        public UnityEvent<IDamagable> onDamage;
        /// <summary>
        ///  CAN TRIGGER ON SAME THING onDamage was called on
        /// </summary>
        public UnityEvent onHit;
        public UnityEvent onTimeout;
        
        private float lastMoveAmount = 0;
        [SerializeField]private bool collidedLastFrame = false;
        private static Collider[] colliderResultCache = new Collider[1];
        
        public void initialize(Vector3 spawnPos, Vector3 dir)
        {
            transform.position = spawnPos;
            ShotDir = dir;
            transform.lookAlongTopDown(dir);

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
                handleTimeout();
            }
        }

        private void handleTimeout()
        {
            Debug.Log("timeout projectle", gameObject);

            onTimeout.Invoke();
            handleEnd();
        }


        private bool queryCollision(Vector3 moveVec,float checkDistance,  out Vector3 hitPoint, out float hitDist, out Collider hitCollider)
        {
            if (Physics.OverlapSphereNonAlloc(transform.position, collisionRadius, colliderResultCache, hitLayer, QueryTriggerInteraction.Collide) > 0)
            {
                hitCollider = colliderResultCache[0];
                hitPoint = hitCollider.ClosestPoint(transform.position);
                hitDist = (hitPoint - transform.position).magnitude;
                return true;
            }
            bool result = Physics.SphereCast(transform.position,
                collisionRadius,
                moveVec,
                out var hit,
                checkDistance,
                hitLayer,
                
                QueryTriggerInteraction.Collide
                );
            hitPoint = hit.point;
            hitCollider = hit.collider;
            hitDist = hit.distance;
            return result;
        }
        
        private void move(float delta)
        {
            Vector3 moveVec = speedCurve.Evaluate(durationTimer.Ratio) *  speed * delta * ShotDir;

            if (affectedByGravity)
            {
                moveVec += Vector3.down * (gravity * Time.deltaTime);
            }

            float moveAmount = moveVec.magnitude;
            Vector3 moveDir = moveVec / moveAmount;
            
            collidedLastFrame = queryCollision(moveDir, moveAmount, 
                out var hitPoint, out var hitDist, out var hitCollider);
            
            if ( collidedLastFrame && hitCollider != null)
            {
                var d = hitCollider.GetComponent<IDamagable>();
                if (d != null)
                {
                    onDamage.Invoke(d);
                }
                handleHit(hitPoint);
                moveVec = hitDist * moveDir;

                transform.position +=  moveVec;

            }
            else
            {
                transform.position += moveVec;

            }
        }

        protected void handleHit(Vector3 point)
        {
            
            /*var particles = GameplayPoolManager.Instance.particlePool
                .get(hitParticlesPrefab);
            particles.transform.position = point;
            particles.transform.right = -transform.right;*/
            
            handleEnd();

            onHit.Invoke();
        }

        public void handleEnd()
        {
            Destroy(gameObject);
        }

        public void initialize()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color =Color.red;
            ;
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
    }
}
