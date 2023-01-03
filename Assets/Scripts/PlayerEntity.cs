using System;
using System.Collections;
using System.Collections.Generic;
using Combat.Pickups;
using UnityEngine;

public class PlayerEntity : LivingEntity
{
    //will add player specific behaviour here if needed
    public PlayerInventory inventory;
    public Transform gunParent;
    public Ray getPlayerShotDirRay()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        return ray;
    }
    
 

    
}
