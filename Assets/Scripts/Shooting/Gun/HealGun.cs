﻿using Combat;
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
        //this is -ve so dealing damage normally will heal targets
        public DamageInfo damagePerHit = new DamageInfo(-20);
        public override void shoot()
        {
            var ray = player.getPlayerShotDirRay();
            if (Physics.Raycast(ray, out var hitResults, shotDist, hitLayerMask))
            {
                trailEffect.enableTrail(shotPoint.position, hitResults.point);

                var damagee = hitResults.collider.GetComponent<IDamagable>();
                if (damagee != null)
                {
                    damagee.takeDamage(damagePerHit);
                }
            }
            else
            {
                trailEffect.enableTrail(shotPoint.position, ray.direction* shotDist);

            }
        }


        public override void handleAddedToInventorySlot(ItemSlot slot)
        {
            base.handleAddedToInventorySlot(slot);
            player = slot.inventory.owner;
            transform.parent = player.gunParent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}