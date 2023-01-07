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
        public Vector3 LookDelta => lookDelta;
        public Vector3 lookDelta; 
        [SerializeField]private Vector2 moveInput;
        public static Vector2 RawMoveInput => Instance.moveInput;
        public static Vector3 NormalizedTopDownMoveInput { get; private set; }
        public static bool IsMoveInputActive { get; private set; } = false;



        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference use1Action;
        [SerializeField] private InputActionReference use2Action;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference itemShiftAction;
        [SerializeField] private InputActionReference pauseMenuAction;
        [SerializeField] private InputActionReference debugAction1;

        public static InputButtonSlot jumpButton = new InputButtonSlot();
        public static InputButtonSlot use1Button = new InputButtonSlot();
        public static InputButtonSlot use2Button = new InputButtonSlot();
        public static InputButtonSlot sprintButton = new InputButtonSlot();
        public static InputButtonSlot pauseButton = new InputButtonSlot();

        public static InputButtonSlot debugButton1 = new InputButtonSlot();

        public SafeEvent<float> itemShift;



        protected override void initialize()
        {
            itemShift = new SafeEvent<float>();
        }

        public static Vector3 convertVecCamRelative(Camera cam, Vector3 dir)
        {
            return Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up) * dir;
        }



        void OnEnable()
        {
            if (inputActionMap == null)
                return;
        
            inputActionMap.Enable();


            jumpButton.bind(jumpAction);
            use1Button.bind(use1Action);
            use2Button.bind(use2Action);
            sprintButton.bind(sprintAction);
            pauseButton.bind(pauseMenuAction);
            
            moveAction.action.performed += OnMovePerformed;
            moveAction.action.canceled += OnMoveCancelled;
            lookAction.action.performed += OnLookPerformed;
            lookAction.action.canceled += OnLookCancelled;
            itemShiftAction.action.performed += handleItemShiftPerformed;
#if UNITY_EDITOR

            debugButton1.bind(debugAction1);

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
            sprintButton.unBind(sprintAction);
            pauseButton.unBind(pauseMenuAction);
#if UNITY_EDITOR

            debugButton1.unBind(debugAction1);

#endif

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

        public static void PlayModeExitCleanUp()
        {
            jumpButton.cleanup();
            use1Button.cleanup();
            use2Button.cleanup();
            sprintButton.cleanup();
            pauseButton.cleanup();
#if UNITY_EDITOR

            debugButton1.cleanup();

#endif
            if (Instance != null)
            {
                Instance.itemShift.clear();
            }
        }
    }
}
