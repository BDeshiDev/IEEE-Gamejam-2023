using System;
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

    public CharacterController cc;
    public BoxCollider collider;
    public Rigidbody rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void handleDeath(ResourceComponent obj)
    {
        base.handleDeath(obj);
        fsm.transitionToState(deathState);
    }


}