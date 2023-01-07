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
    [SerializeField] private State deathState;
    [SerializeField] private State goombaMoveForwardState;
    [SerializeField] private DamageInfo contactDamage;
    [SerializeField] private FiniteTimer contactDamageTImer = new FiniteTimer(0, 1.2f);
    public CharacterController cc;
    public BoxCollider collider;
    [SerializeField]private MeshRenderer renderer;
    public bool startAsDead = false;
    // public Rigidbody rb;


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

    public void setFaceMat(Material mat)
    {
        Material[] materials = renderer.materials;
        materials[3] = mat;
        renderer.materials = materials;
        Debug.Log(mat + " " + renderer.materials[3]);
        foreach (var m in renderer.materials)
        {
            Debug.Log("m = " + m);
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