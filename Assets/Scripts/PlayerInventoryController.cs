using System.Collections;
using System.Collections.Generic;
using Combat.Pickups;
using Core.Input;
using UnityEngine;

/// <summary>
/// Binds inventory item usages to input buttons
/// </summary>
public class PlayerInventoryController : MonoBehaviour
{
    private PlayerInventory inventory;
    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        InputManager.use1Button.addPerformedCallback(gameObject, handleUse1);     
        InputManager.use2Button.addPerformedCallback(gameObject, handleUse2);  
        InputManager.Instance.itemShift.add(gameObject, handleItemShift);
    }

    private void handleItemShift(float shiftDirection)
    {
        Debug.Log("shiftDirection" + shiftDirection);
        inventory.shiftSelectedItem(shiftDirection > 0);
    }

    private void handleUse1()
    {
        inventory.useCurrentPickup1();
    }
    
    private void handleUse2()
    {
        inventory.useCurrentPickup2();
    }
}
