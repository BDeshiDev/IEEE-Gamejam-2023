using System;
using Combat;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

    public event Action EntityDied;

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public Transform getTransform()
    {
        return transform;
    }

    public void forceKill()
    {
        healthComponent.forceEmpty();
    }

    public virtual void takeDamage(DamageInfo damage)
    {
        healthComponent.modifyAmount(-damage.healthDamage);
    }

    protected virtual void Start()
    {
        healthComponent.Emptied += handleDeath;
    }

    protected virtual void OnDestroy()
    {
        healthComponent.Emptied -= handleDeath;
    }
    protected virtual void handleDeath(ResourceComponent obj)
    {
        Debug.Log(gameObject + " is dead" ,gameObject);
        // inform the subsribed things that this is dead 
        EntityDied?.Invoke();
    }

}