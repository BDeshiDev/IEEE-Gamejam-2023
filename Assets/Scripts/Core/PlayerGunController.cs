using System;
using Core.Input;
using UnityEngine;

namespace Core.Misc.Core
{
    public class PlayerGunController: MonoBehaviour
    {
        [SerializeField] Gun gun;

        private void Start()
        {
            InputManager.shootButton.addPerformedCallback(gameObject, gun.shoot);
        }
    }
}