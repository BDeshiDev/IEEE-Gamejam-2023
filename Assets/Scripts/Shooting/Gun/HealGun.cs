using Combat.Pickups;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Misc.Core
{
    public class HealGun : Gun
    {
        public Transform shotPoint;
        public float shotDist = 9999;
        public LayerMask hitLayerMask;

        public GunShotTrailEffect trailEffect;
        [SerializeField] private PlayerEntity player;
        public override void shoot()
        {
            var ray = player.getPlayerShotDirRay();
            if (Physics.Raycast(ray, out var hitResults, shotDist, hitLayerMask))
            {
                trailEffect.enableTrail(shotPoint.position, hitResults.point);
            }
            else
            {
                trailEffect.enableTrail(shotPoint.position, ray.direction* shotDist);

            }
        }


        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            player = slot.inventory.owner;
            transform.parent = player.gunParent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}