using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerMovement : CharacterCore, IPersistentData {

    [Header("Player movement")]

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

    }

    private void OnDisable() {

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
        SelectState();
        stateMachine.CurrentState.DoBranch();
    }

    void FixedUpdate() {
        HandleHorizontalMovement();
        ApplyHorizontalFriction();
        HandleGravityScale();
        FaceMovementDirection();
        stateMachine.CurrentState.FixedDoBranch();

        LimitWalkingSpeed();
    }

    public void HandleHorizontalMovement() {
        if (DisableHorizontalMovement) return;
        rigidBody.linearVelocityX += movementParams.HorizontalAcceleration * input.HorizontalMovement;
    }

    public void HandleGravityScale() {
        if (GravityOverride) return;
        else {
            ApplyAdaptiveGravity();
        }
    }

    private void SelectState() {
        if (IsGrounded) {
            stateMachine.Set(groundedState);
        }
        else {
            if (IsGrabbingLedge()) {
                print("Entering ledge grab state");
                //machine.Set(ledgeGrabState);
            }
            else if (IsGrabbingWall && rigidBody.linearVelocityY <= 0) {
                stateMachine.Set(onWallState);
            }
            else {
                stateMachine.Set(airborneState);
            }
        }
    }

    #region Jump Methods

    public override bool CanJump() {
        return IsGrounded || CanCoyoteJump() || CanWallJump();
    }

    private bool CanWallJump() {
        return IsTouchingWall && rigidBody.linearVelocityY <= 0;
    }

    private bool CanCoyoteJump() {
        return TimeSinceGrounded < movementParams.CoyoteTime && rigidBody.linearVelocityY <= 0;
    }

    private void HandleJumpInput() {

        if (input.Jump && CanJump()) {
            Jump();
        }

    }

    private void Jump() {
        rigidBody.linearVelocityY = movementParams.JumpSpeed;
    }

    #endregion

    #region Wall Hang Methods

    public bool IsGrabbingLedge() {
        //TODO: implement ledge grab
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