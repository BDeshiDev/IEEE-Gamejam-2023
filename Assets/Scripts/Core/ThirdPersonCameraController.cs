using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Input;
using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    private GameObject _mainCamera;
    // cinemachine
    [SerializeField]private float _cinemachineTargetYaw;
    [SerializeField]private float _cinemachineTargetPitch;
    
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    private const float _threshold = 0.01f;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (InputManager.LookDelta.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            _cinemachineTargetYaw += InputManager.LookDelta.x * Time.deltaTime;
            _cinemachineTargetPitch += InputManager.LookDelta.y * Time.deltaTime;
            Debug.Log(_cinemachineTargetYaw + " " + _cinemachineTargetPitch);

        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        Debug.Log(_cinemachineTargetYaw + " 2222 " + _cinemachineTargetPitch);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }
    
    
    private void LateUpdate()
    {
        CameraRotation();
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}
