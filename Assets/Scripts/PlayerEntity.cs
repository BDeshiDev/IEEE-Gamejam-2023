using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;
    private void Start()
    {
        healthComponent.Emptied += handleDeath;
    }

    private void OnDestroy()
    {
        healthComponent.Emptied -= handleDeath;
    }

    private void handleDeath(ResourceComponent obj)
    {
        Debug.Log("dead");
    }
}
