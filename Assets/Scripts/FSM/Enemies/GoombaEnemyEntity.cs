using System;
using BDeshi.Utility;
using Combat;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Moves forwards until it hits a wall, turns around and starts movign again
/// </summary>
public class GoombaEnemyEntity : LivingEntity
{
    [SerializeField]private FiniteStateMachine fsm;
    private Animator animator;
    public Animator Animator => animator;
    [SerializeField]private State deathState;
    [SerializeField]private State goombaMoveForwardState;
    [SerializeField] private DamageInfo contactDamage;
    [SerializeField] private FiniteTimer contactDamageTImer = new FiniteTimer(0, 1.2f);
    public CharacterController cc;
    public BoxCollider collider;

    public bool startAsDead = false;
    // public Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var d = other.GetComponent<IDamagable>();
        if (d != null)
        {
            Debug.Log("d " + d);
            d.takeDamage(contactDamage);
        }
    }

    private void Update()
    {
        if (startAsDead)
        {
            startAsDead = false;
            forceKill();
        }
    }


    protected override void handleDeath(ResourceComponent obj)
    {
        base.handleDeath(obj);
        fsm.transitionToState(deathState);
    }

    public override void takeDamage(DamageInfo damage)
    {
        base.takeDamage(damage);
        if (fsm.CurState == deathState && damage.isHeal)
        {
            if (HealthComponent.IsFull)
            {
                //revive
                fsm.transitionToState(goombaMoveForwardState);
            }
        }
    }
}