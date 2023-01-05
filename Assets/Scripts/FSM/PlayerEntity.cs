using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Combat;
using Combat.Pickups;
using Core.Misc.Core;
using UnityEngine;

public class PlayerEntity : LivingEntity
{
    //will add player specific behaviour here if needed
    public PlayerInventory inventory;
    public Transform gunParent;
    public FPSCameraController camController;
    public SimpleCharacterController cc;

    /// <summary>
    /// static event for any player death
    /// So that it works across scenes
    /// </summary>
    public static SafeEvent<PlayerEntity> playerDied = new SafeEvent<PlayerEntity>();
    
    
    public Ray getPlayerShotDirRay()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        return ray;
    }

    protected override void handleDeath(ResourceComponent obj)
    {
        base.handleDeath(obj);
        playerDied.Invoke(this);
    }

    public static void PlayModeExitCleanUp()
    {
        playerDied.clear();
    }

    public override void takeDamage(DamageInfo damage)
    {
        base.takeDamage(damage);
        if (Mathf.Abs(damage.knockbackMagitude) > 0)
        {
            cc.addSpeedBoost(damage.damageKnockbackDir * damage.knockbackMagitude);
        }

        if (damage.resetJump)
        {
            cc.resetJumpLimit();
        }

        if (damage.resetGravity)
        {
            cc.applyGravity();
        }
    }
}
