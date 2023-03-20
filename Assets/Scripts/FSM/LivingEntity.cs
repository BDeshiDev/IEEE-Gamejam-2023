using System;
using Combat;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

    public event Action EntityDied;
    public event Action<DamageInfo> TookDamage;
    

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
    /// <summary>
    /// base version only applies damage to health comp
    /// </summary>
    /// <param name="damage"></param>
    public virtual void takeDamage(DamageInfo damage)
    {
        healthComponent.modifyAmount(-damage.healthDamage);
        TookDamage?.Invoke(damage);
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