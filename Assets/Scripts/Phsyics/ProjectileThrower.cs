using UnityEngine;

namespace Combat
{
    public class ProjectileThrower: MonoBehaviour
    {
        public RaycastProjectile healthPackProjectilePrefab;

        public Vector3 getShotDir(PlayerEntity player)
        {
            var ray = player.getPlayerShotDirRay();
            if(Physics.Raycast(ray, out var hitResults, Mathf.Infinity, healthPackProjectilePrefab.hitLayer))
            {
                return (hitResults.point - player.transform.position).normalized;
            }
            return ray.direction;
        }
        
        public void throwProjectile(PlayerEntity player)
        {
            var proj = Instantiate(healthPackProjectilePrefab);
            proj.initialize(player.transform.position,getShotDir(player));
        }

    }
}