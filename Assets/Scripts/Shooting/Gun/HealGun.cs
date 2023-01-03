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

            if (Physics.Raycast(player.getPlayerShotDirRay(), out var hitResults, shotDist, hitLayerMask))
            {
                trailEffect.enableTrail(shotPoint.position, hitResults.point);
            }
            else
            {
                trailEffect.enableTrail(shotPoint.position, player.transform.forward * shotDist);

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