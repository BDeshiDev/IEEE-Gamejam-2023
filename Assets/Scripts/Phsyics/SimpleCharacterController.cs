using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Input;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using Core.Input;
using Core.Misc;
using FSM.GameState;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleCharacterController : MonoBehaviour
{
    private CharacterController cc;
    public Vector3 input;
    [SerializeField] private Vector3 inputVel;
    [SerializeField] Vector3 moveVel = Vector3.zero;
    
    [Header("movement")]
    [SerializeField] private float sprintSpeedMultiplier = 1.6f;
    [FormerlySerializedAs("moveSpeed")] 
    [SerializeField] private float baseMoveSpeed = 6;
    [SerializeField] private float curMoveSpeed = 6;
    [SerializeField] Vector3 maxVelocityInEachAxis = Vector3.one * 40;
    [SerializeField] Vector3 minVelocityInEachAxis;
    
    [SerializeField] private float rotationSpeed = 60;
    
    [SerializeField] private bool IsGrounded;
    [Header("gravity/jump")]
    [SerializeField] private float gravity = -15.15f;
    [SerializeField] private float maxJumpHeight = 5;
    [SerializeField] private float minJumpHeight = 2;
    [SerializeField] private float timeToJumpApex = .6f;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    
    


    public float jumpGracePeriod = .2f;

    
    [SerializeField] bool isJumping = false;
    public int jumpLimit = 1;
    [SerializeField] int remainingJumps;
    public bool GravityEnabled = true;

    [Header("boosting")]
    public Vector3 boostBuildUp;
    public Vector3 maxBoostMagnitudePerAxis =Vector3.one* 20; 
    public float boostDecay = -2f;
    public float minBoostThreshold = .15f;
    public FiniteTimer boostGravityImmunityTimer = new FiniteTimer(0, .6f);
    
    [Header("groundcheck")]
    public float groundCheckRadius = .5f;
    [FormerlySerializedAs("groundHeight")] [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundLayerMask;

    private Camera cam;
    // public Animator animator;

    private void OnValidate()
    {
        recalcJumpAndGravity();
    }

    #region Input

    public void calcInput(out Vector3 moveInput)
    {
        moveInput = InputManager.convertVecCamRelative(
            cam, 
            InputManager.RawMoveInput.toTopDown()
            );

        moveInput.Normalize();

        curMoveSpeed = baseMoveSpeed * (InputManager.sprintButton.isHeld ? sprintSpeedMultiplier : 1);
    }

    public void clearBoost()
    {
        boostBuildUp = Vector3.zero;
    }
    public void addBoost(Vector3 boost)
    {
        // moveVel += boost;
        // addKnockBack(boost);
        Debug.Log("add boost " + boost);
        boostBuildUp += boost;
        clampVector(ref boostBuildUp, maxBoostMagnitudePerAxis);

        boostGravityImmunityTimer.reset();
        
    }
 
    /// <summary>
    /// We can have boosts in different directions
    /// Ex: boost walls and spikes
    /// They should coexists as long as the axes are diff
    /// </summary>
    void clampVector(ref Vector3 vec, Vector3 maxPerAxis)
    {
        vec.x = Mathf.Sign(vec.x) * Mathf.Min(Mathf.Abs(vec.x), maxPerAxis.x);
        vec.y = Mathf.Sign(vec.y) * Mathf.Min(Mathf.Abs(vec.y), maxPerAxis.y);
        vec.z = Mathf.Sign(vec.z) * Mathf.Min(Mathf.Abs(vec.z), maxPerAxis.z);
    }
    
    /// <summary>
    /// We can have boosts in different directions
    /// Ex: boost walls and spikes
    /// They should coexists as long as the axes are diff
    /// </summary>
    void clampVector(ref Vector3 vec, Vector3 maxPerAxis, Vector3 minPerAxis)
    {
        vec.x = Mathf.Clamp(vec.x, minPerAxis.x, maxPerAxis.x);
        vec.y = Mathf.Clamp(vec.y, minPerAxis.y, maxPerAxis.y);
        vec.z = Mathf.Clamp(vec.z, minPerAxis.z, maxPerAxis.z);
    }

    public void calcVelocity(Vector3 moveInput)
    {
        float yVel = moveVel.y;
        inputVel = moveInput * curMoveSpeed;
        
        //turn off acceleration
        // if (moveInput == Vector3.zero)
        // {
        //     moveVel = Vector3.SmoothDamp(moveVel, inputVel, ref curAcc, velSmoothTime, maxAcc);
        // }
        // else
        // {
        //     moveVel = Vector3.SmoothDamp(moveVel, inputVel, ref curAcc, velSmoothTime, maxAcc);
        // }
        moveVel = inputVel;

        moveVel.y = yVel;
        
        if (IsGrounded)
        {
            moveVel.y = Mathf.Max(0, moveVel.y);
        }

        applyJumpVel();
        applyGravity();
    }



    #endregion
    # region  Jump and gravity

    private Vector3? groundPoint;

    Vector3 getGroundCheckEndPoint()
    {
        return getGroundCheckStartPoint()+ (groundCheckDistance - groundCheckRadius) * Vector3.down;
    }

    private Collider[] groundCheckCache = new Collider[1];
    public bool checkGrounded()
    {
        var startPoint = getGroundCheckStartPoint();
        if (Physics.OverlapSphereNonAlloc(startPoint, groundCheckRadius,groundCheckCache, groundLayerMask) > 0)
        {
            //this doesn't update the ground point yet
            return true;
        }
        else
        {
            Physics.SphereCast(getGroundCheckStartPoint(), 
                groundCheckRadius, 
                Vector3.down, 
                out var hit, 
                groundCheckDistance - groundCheckRadius,
                groundLayerMask);
        
            if (hit.collider)
            {
                groundPoint = hit.point;
            }
            else
            {
                groundPoint = null;
            }

            return hit.collider != null;
        }
    }

    private void Start()
    {
        recalcJumpAndGravity();
    }

    public void recalcJumpAndGravity()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);


    }

    public void applyJumpVel()
    {
        // the actual jump limit is refreshed whe you are grounded
        // So you can't jump infinitely midair as that defeats any sense of challenge
        if (IsGrounded)
        {
            resetJumpLimit();
        }
        

        if (!isJumping)
        {
            if (!IsGrounded // can only jump when NOT GROUNDED
                && InputManager.jumpButton.isHeld)
                // && InputManager.jumpButton.LastPressedTime.withinDuration(jumpGracePeriod))
            {
                if (remainingJumps > 0)
                {
                    remainingJumps -= 1;
                    handleJumpDown();   
                }
            }
        }else
        {
            if (!InputManager.jumpButton.isHeld)
            {
                handleJumpUp();
            }
        }
        // if (!IsGrounded // can only jump when NOT GROUNDED
        //     && InputManager.jumpButton.isHeld
        //     // && InputManager.jumpButton.LastPressedTime.withinDuration(jumpGracePeriod)
        //     && !isJumping)
        // {
        //     if (remainingJumps > 0)
        //     {
        //         remainingJumps -= 1;
        //         handleJumpDown();   
        //     }
        // }
        // else
        // {
        //     if (!InputManager.jumpButton.isHeld)
        //     {
        //         handleJumpUp();
        //     }
        // }
    }

    public void resetJumpLimit()
    {
        remainingJumps = jumpLimit;
    }

    public void handleJumpDown()
    {
        Debug.Log("moveVel.y = " + moveVel.y + " maxjumpvel "  + maxJumpVelocity);   

        moveVel.y = (moveVel.y >= 0 ? moveVel.y : 0) + maxJumpVelocity;
        Debug.Log("moveVel.y = " + moveVel.y);   
        isJumping = true;
    }
    
    public void handleJumpUp()
    {
        moveVel.y = Mathf.Min(moveVel.y, minJumpVelocity);
        isJumping = false;
    }

    public void applyGravity()
    {
        if(IsGrounded|| !GravityEnabled)
            return;
        moveVel.y +=  gravity * Time.deltaTime;
    }


    public void resetFall()
    {
        moveVel.y = Mathf.Max(moveVel.y, 0);
    }
    
    #endregion
    #region Knockback

    public float groundCheckOffset = -.5f;
    [SerializeField]private int inputCancelBoostFactor = 10;


    Vector3 getGroundCheckStartPoint()
    {
        return transform.position + groundCheckOffset * Vector3.up;
    }
 
    #endregion

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        resetJumpLimit();
        cam = Camera.main;
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsPaused)
        {
            return;
        }
        IsGrounded = checkGrounded();
        calcInput(out input);
        calcVelocity(input);
        calcMoveAmount(out var moveAmount);

        applyMovement(moveAmount);
        // updateRotation();
        // animate();
    }

    void applyMovement(Vector3 moveAmount)
    {
        cc.Move(moveAmount);
    }

    private void updateRotation()
    {
        var lookDir = moveVel;
        lookDir.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    private void animate()
    {
        // animator.SetFloat("Speed", moveVel.magnitude / baseMoveSpeed);
        // animator.SetBool("Grounded", IsGrounded);
        // animator.SetBool("Jump" , isJumping);
        // animator.SetBool("IsMoving" , InputManager.RawMoveInput != Vector2.zero);
        // animator.SetBool("FreeFall" , moveVel.y <0 && IsGrounded);
    }


    

    public void calcMoveAmount( out Vector3 moveAmount)
    {
        var actualMoveVel = moveVel;
        
        actualMoveVel += boostBuildUp;
        updateBoost();
        clampVector(ref actualMoveVel, maxVelocityInEachAxis, minVelocityInEachAxis);

        
        moveAmount = actualMoveVel * Time.deltaTime;
    }

    private void updateBoost()
    {
        
        if (boostBuildUp.magnitude > minBoostThreshold)
        {
            float dotWithInputVel = Vector3.Dot(input, boostBuildUp);


            boostBuildUp -= boostBuildUp.normalized * (boostDecay * (dotWithInputVel < 0? inputCancelBoostFactor:1) * Time.deltaTime);
        }
        else
        {
            boostBuildUp = Vector3.zero;
        }
        boostGravityImmunityTimer.safeUpdateTimer(Time.deltaTime);
        if (boostGravityImmunityTimer.isComplete)
        {
            if (IsGrounded)
            {
                boostBuildUp.y = Mathf.Min(0, boostBuildUp.y);
            }
        }
    }

    public void teleportTo(Vector3 pos)
    {
        cc.enabled = false;
        transform.position = pos;
        cc.enabled = true;
    }

    public void stop()
    {
        moveVel = Vector3.zero;
    }
    

    public void lookAlong(Vector3 dir)
    {
        transform.lookAlongTopDown(dir);
    }

    public void setRotation(Quaternion r)
    {
        transform.rotation = r;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundPoint.HasValue)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(groundPoint.Value, .25f);
        }
        else
        {
            Gizmos.color = Color.red;
        }

        var startPoint = getGroundCheckStartPoint();
        var endPoint = getGroundCheckEndPoint();
        Gizmos.DrawWireSphere(startPoint, groundCheckRadius);
        Gizmos.DrawWireSphere(endPoint, groundCheckRadius);
        Gizmos.DrawSphere(startPoint, .125f );
        Gizmos.DrawSphere(endPoint, .125f );

        Gizmos.DrawLine(startPoint, endPoint);
        
    }
}
