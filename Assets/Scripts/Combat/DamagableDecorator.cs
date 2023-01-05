using System;
using UnityEngine;

namespace Combat
{
    //allow getComponent<Damagable> on this object (get via collider or other methods) to be forwarded to another object
    // this is useful if we want to give the player a different trigger collider for something.
    public class DamagableDecorator: MonoBehaviour, IDamagable
    {
        private IDamagable decoratee;

        private void Awake()
        {
            decoratee = transform.parent.GetComponent<IDamagable>();
        }

        public GameObject getGameObject()
        {
            return decoratee.getGameObject();
        }

        public Transform getTransform()
        {
            return decoratee.getTransform();
        }

        public void takeDamage(DamageInfo damage)
        {
            decoratee.takeDamage(damage);
        }
    }
}