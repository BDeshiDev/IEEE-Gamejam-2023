using System;
using Core.Input;
using UnityEngine;

namespace Core.Misc.Core
{
    public class FPSCameraController:MonoBehaviour
    {
        [SerializeField]private float xRotation;
        private Transform PlayerBody;
        public float mouseSensitivity = 5;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Awake()
        {
            PlayerBody = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            Vector2 mouseDelta = InputManager.Instance.lookDelta * (mouseSensitivity * Time.deltaTime);
            xRotation -= mouseDelta.y;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            PlayerBody.Rotate(Vector3.up * mouseDelta.x);
        }
    }
}