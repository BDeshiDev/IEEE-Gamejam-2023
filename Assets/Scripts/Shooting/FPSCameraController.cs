using System;
using Core.Input;
using FSM.GameState;
using UnityEngine;

namespace Core.Misc.Core
{
    public class FPSCameraController:MonoBehaviour
    {
        [SerializeField]private float xRotation;
        private Transform PlayerBody;
        public static float mouseSensitivity = 50;
        public static float FOV = 60;
        private Camera cam;
        public void setFOV(float fov)
        {
            FOV = fov;
            cam.fieldOfView = FOV;
        }
        

        
        
        
        

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Awake()
        {
            PlayerBody = GameObject.FindWithTag("Player").transform;
            cam = GetComponent<Camera>();
            
            setFOV(FOV);
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