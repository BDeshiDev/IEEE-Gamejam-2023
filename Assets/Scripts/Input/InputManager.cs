using System;
using BDeshi.Input;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Core.Input
{

    public class InputManager : MonoBehaviourSingletonPersistent<InputManager>
    {
        [SerializeField] private InputActionAsset inputActionMap;
        public Transform AimOrigin;
        [SerializeField] private  Vector3 aimDir; 
        public static Vector3 AimDir => Instance.aimDir;
        public static bool IsAimActive => Instance.aimDir != Vector3.zero;
        [SerializeField] private Camera cam;

        public bool MouseAimActive = false;
        public bool GamePadAimActive = false;
        public Vector3 LookDelta => lookDelta;
        public Vector3 lookDelta; 
        public static Vector3 NormalizedTopDownAimInput { get; private set; }


        public static Vector3 NormalizedTopDownAimEndPoint => normalizedTopDownAimEndPoint;
        private static Vector3 normalizedTopDownAimEndPoint;

        [SerializeField] private LayerMask aimLayer;

        [SerializeField]private Vector2 moveInput;
        public static Vector2 RawMoveInput => Instance.moveInput;
        public static Vector3 NormalizedTopDownMoveInput { get; private set; }
        public static bool IsMoveInputActive { get; private set; } = false;


        public bool applySensitivity = true;
        public float gamepadVel = 80;
        private float dotFactor = 3;
        private Vector2 gamepadVal = Vector2.zero;


        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference use1Action;
        [SerializeField] private InputActionReference use2Action;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference itemShiftAction;
        public static InputButtonSlot jumpButton = new InputButtonSlot();
        public static InputButtonSlot use1Button = new InputButtonSlot();
        public static InputButtonSlot use2Button = new InputButtonSlot();
        
        public SafeEvent<float> itemShift;



        // void Update()
        // {
        //     updateAim();
        // }

        protected override void initialize()
        {
            cam = Camera.main;
            Debug.Log(cam,cam);
            itemShift = new SafeEvent<float>();
        }

        public static Vector3 convertVecCamRelative(Vector3 dir)
        {
            return Quaternion.AngleAxis(Instance.cam.transform.rotation.eulerAngles.y, Vector3.up) * dir;
        }

        public static float convertVecToAngleCamRelative(Vector3 dir){
           return Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Instance.cam.transform.eulerAngles.y;
        }


        private void AimAlongPerformed(InputAction.CallbackContext obj)
        {
            GamePadAimActive = true;
            MouseAimActive = false;

            gamepadVal = Vector2.zero;
        }
        private void AimAlongCancelled(InputAction.CallbackContext obj)
        {
            GamePadAimActive = false;
        }

        private void OnAimAtPerformed(InputAction.CallbackContext obj)
        {
            MouseAimActive = true;
            GamePadAimActive = false;
        }

        void OnAimAtCancelled(InputAction.CallbackContext c)
        {
            MouseAimActive = false;
        }
        

        void OnEnable()
        {
            if (inputActionMap == null)
                return;
        
            inputActionMap.Enable();


            jumpButton.bind(jumpAction);
            use1Button.bind(use1Action);
            use2Button.bind(use2Action);
            
            moveAction.action.performed += OnMovePerformed;
            moveAction.action.canceled += OnMoveCancelled;
            lookAction.action.performed += OnLookPerformed;
            lookAction.action.canceled += OnLookCancelled;
            itemShiftAction.action.performed += handleItemShiftPerformed;
#if UNITY_EDITOR

            // debugButton1.bind(debugAction1);

#endif

        }

        private void handleItemShiftPerformed(InputAction.CallbackContext obj)
        {
            float delta = obj.ReadValue<float>();
            itemShift.Invoke(delta);
        }


        void OnDisable()
        {
            if(inputActionMap == null)
                return;


            jumpButton.unBind(jumpAction);
            use1Button.unBind(use1Action);
            use2Button.unBind(use2Action);

            moveAction.action.performed -= OnMovePerformed;
            moveAction.action.canceled -= OnMoveCancelled;
            lookAction.action.performed -= OnLookPerformed;
            lookAction.action.canceled -= OnLookCancelled;

            inputActionMap.Disable();
        }

        private void OnLookPerformed(InputAction.CallbackContext obj)
        {
            lookDelta = obj.ReadValue<Vector2>();
        }
        
        private void OnLookCancelled(InputAction.CallbackContext obj)
        {
            lookDelta = Vector3.zero;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            moveInput = obj.ReadValue<Vector2>();
            NormalizedTopDownMoveInput = moveInput.normalized.toTopDown();
            IsMoveInputActive = true;
        }
        
        private void OnMoveCancelled(InputAction.CallbackContext obj)
        {
            NormalizedTopDownMoveInput = moveInput = Vector2.zero;
            IsMoveInputActive = false;
        }

        private void OnDrawGizmosSelected()
        {
            if(AimOrigin)
                Gizmos.DrawRay(AimOrigin.position, aimDir);
        }
        
        public static void PlayModeExitCleanUp()
        {
            jumpButton.cleanup();
            use1Button.cleanup();
            use2Button.cleanup();

            if (Instance != null)
            {
                Instance.itemShift.clear();
            }
        }
    }
}
