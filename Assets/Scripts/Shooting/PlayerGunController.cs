using System;
using Core.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Misc.Core
{
    public class PlayerGunController: MonoBehaviour
    {
        [FormerlySerializedAs("gun")] [SerializeField] HealGun healGun;

        private void Start()
        {
            InputManager.shootButton.addPerformedCallback(gameObject, healGun.shoot);
        }
    }
}