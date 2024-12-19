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
        stateMachine.CurrentState.FixedDoBranch(); 
        HandleCoreMovement();
    }

    public void HandleHorizontalMovement() {
        HorizontalInput = IsWallJumping ? Mathf.Sign(rigidBody.linearVelocityX) : input.HorizontalMovement;
        rigidBody.linearVelocityX += movementParams.HorizontalAcceleration * HorizontalInput;
    }

    // MOVE TO AIRBORNE STATE AND JUMP STATE RESPECTIVELY
    public void HandleGravityScale() {
        if (GravityOverride) return;
        if (input.CancelJump) {
            rigidBody.gravityScale = movementParams.FallingGravity;
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

    private bool CanJump() {
        return IsGrounded || CanCoyoteJump();
    }

    private bool CanCoyoteJump() {
        return TimeSinceGrounded < movementParams.CoyoteTime && rigidBody.linearVelocityY <= 0;
    }

    private void HandleJumpInput() {
        if (input.Jump) {
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
        rigidBody.linearVelocityY = movementParams.JumpSpeed;
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
        return IsTouchingWall && input.Grab;
    }

    private bool IsGrabbingLedge() {
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