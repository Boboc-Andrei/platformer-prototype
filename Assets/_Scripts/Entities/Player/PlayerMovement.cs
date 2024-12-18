using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : CharacterCore, IPersistentData {

    [Header("Player movement")]
    public InputManager input;
    public bool JumpEndedEarly;
    public float JumpBufferTime;
    public float jumpBufferCounter;
    public bool queueJump;
    public bool IsJumping;
    public bool IsWallJumping;

    [Header("States")]
    [SerializeField] private AirborneState airborneState;
    [SerializeField] private GroundedState groundedState;
    [SerializeField] private OnWallState onWallState;
    [SerializeField] private OnLedgeState onLedgeState;

    [Header("Ledge sensors")]

    private Vector3 startingPosition;
    private Vector3 startingVelocity = Vector3.zero;

    #region Event Subscription
    private void OnEnable() {
        input.OnJumpPressed += OnJumpPressed;
        input.OnJumpReleased += OnJumpReleased;
    }

    private void OnDisable() {
        input.OnJumpPressed -= OnJumpPressed;
        input.OnJumpReleased -= OnJumpReleased;
    }
    #endregion

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        startingPosition = transform.position;
    }

    private void Start() {
        SetupStates();
        stateMachine.Set(groundedState);

    }

    void Update() {
        HandleJumpInput();
        UpdateJumpFlags();
        SelectState();
        stateMachine.CurrentState.Do();
    }

    void FixedUpdate() {
        HandleHorizontalMovement();
        HandleGravityScale();
        stateMachine.CurrentState.FixedDo(); 
        HandleCoreMovement();
    }

    public void HandleHorizontalMovement() {
        HorizontalInput = IsWallJumping ? Mathf.Sign(rigidBody.linearVelocityX) : input.HorizontalMovementInput;
        rigidBody.linearVelocityX += movementParams.HorizontalAcceleration * HorizontalInput;
    }

    // MOVE TO AIRBORNE STATE AND JUMP STATE RESPECTIVELY
    public void HandleGravityScale() {
        if (GravityOverride) return;
        if (JumpEndedEarly) {
            rigidBody.gravityScale = movementParams.FallingGravity;
            JumpEndedEarly = false;
        }
        else {
            ApplyAdaptiveGravity();
        }
    }

    private void SelectState() {
        if (IsGrounded) {
            stateMachine.Set(groundedState);
        }
        else {
            if(IsGrabbingLedge()) {
                print("Entering ledge grab state");
                //machine.Set(ledgeGrabState);
            }
            else if (IsGrabbingWall() && rigidBody.linearVelocityY <= 0) {
                stateMachine.Set(onWallState);
            }
            else {
                stateMachine.Set(airborneState);
            }
        }
    }


    #region Jump Methods
    private void OnJumpPressed() {
        queueJump = true;
        jumpBufferCounter = JumpBufferTime;
    }

    // MOVE TO JUMP STATE IF POSSIBLE
    private void OnJumpReleased() {
        if (IsJumping && rigidBody.linearVelocityY > 0) {
            EndJumpEarly();
        }
    }

    private bool CanJump() {
        return IsGrounded || CanCoyoteJump();
    }

    private bool CanCoyoteJump() {
        return TimeSinceGrounded < movementParams.CoyoteTime && rigidBody.linearVelocityY <= 0;
    }

    private void HandleJumpInput() {
        if (queueJump) {
            if (CanJump()) {
                Jump();
            }
            else if (IsTouchingWall) {
                WallJump();
            }
        }
    }

    // MOVE TO JUMP STATE ?
    private void Jump() {
        IsJumping = true;
        queueJump = false;
        JumpEndedEarly = false;
        rigidBody.linearVelocityY = movementParams.JumpSpeed;
    }

    // MOVE TO JUMP STATE ?
    private void EndJumpEarly() {
        JumpEndedEarly = true;
        rigidBody.linearVelocityY *= movementParams.JumpCutoffFactor;
    }

    private void UpdateJumpFlags() {
        if (IsGrounded && rigidBody.linearVelocityY <= 0) {
            JumpEndedEarly = false;
            IsJumping = false;
        }
        if (queueJump) {
            jumpBufferCounter -= Time.deltaTime;
            if (jumpBufferCounter < 0) {
                queueJump = false;
            }
        }
    }

    // MOVE TO WALL JUMP STATE ?
    private void WallJump() {
        int nearestWallDirection = IsTouchingLeftWall ? 1 : -1;
        rigidBody.linearVelocityX = nearestWallDirection * movementParams.HorizontalTopSpeed;
        StartCoroutine(DisableMovementForSeconds(0.3f));
        Jump();
    }

    // MOVE TO WALL JUMP STATE ?
    private IEnumerator DisableMovementForSeconds(float time) {
        IsWallJumping = true;
        yield return new WaitForSeconds(time);
        IsWallJumping = false;
    }

    #endregion

    #region Wall Hang Methods
    private bool IsGrabbingWall() {
        return IsTouchingWall && input.GrabPressed;
    }

    private bool IsGrabbingLedge() {
        print("ledge grab still not implemented");
        return false;
    }


    #endregion

    #region Data Persistence
    public void LoadPersistentData(GameData data) {
        rigidBody.position = data.playerPosition;
        rigidBody.linearVelocity = data.playerVelocity;
    }

    public void SavePersistentData(ref GameData data) {
        data.playerPosition = rigidBody.position;
        data.playerVelocity = rigidBody.linearVelocity;
    }

    public void SetDefaultPersistentData(ref GameData data) {
        data.playerPosition = startingPosition;
        data.playerVelocity = startingVelocity;
    }
    #endregion
}