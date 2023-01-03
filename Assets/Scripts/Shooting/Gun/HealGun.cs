using UnityEngine;

namespace Core.Misc.Core
{
    public class HealGun : Gun
    {
        public Transform shotPoint;
        public float shotDist = 9999;
        public LayerMask hitLayerMask;

        public GunShotTrailEffect trailEffect;
        
        public override void shoot()
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out var hitResults, shotDist, hitLayerMask))
            {
                trailEffect.enableTrail(shotPoint.position, hitResults.point);
            }
        }
        
        
    }
}