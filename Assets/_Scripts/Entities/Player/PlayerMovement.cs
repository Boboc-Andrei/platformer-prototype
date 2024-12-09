using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : CharacterCore, IPersistentData {

    [Header("Components")]
    public LayerMask groundMask;
    public InputManager input;


    [Header("Sensorial data")]
    public bool JumpEndedEarly;


    [Header("States")]
    [SerializeField] private IdleState idleState;
    [SerializeField] private WalkState walkState;
    [SerializeField] private AirborneState airborneState;
    [SerializeField] private WallSlideState wallSlideState;

    [Header("Player movement")]
    public float JumpBufferTime;
    public float jumpBufferCounter;
    public bool queueJump;
    public bool IsJumping;
    public bool IsWallJumping;

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
        SetupInstances();
        machine.Set(idleState);

    }

    void Update() {
        HandleJumpInput();
        UpdateJumpFlags();

        SelectState();
        machine.CurrentState.Do();
    }

    void FixedUpdate() {
        HandleHorizontalMovement();
        HandleGravityScale();
        machine.CurrentState.FixedDo();
        HandleCoreMovement();
    }

    public void HandleHorizontalMovement() {
        if (IsCrowdControlled) return;
        HorizontalHeading = IsWallJumping ? Mathf.Sign(body.linearVelocityX) : input.HorizontalMovementInput;
        body.linearVelocityX += movementParams.HorizontalAcceleration * HorizontalHeading;
    }

    public void HandleGravityScale() {
        if (GravityOverride) return;
        if (JumpEndedEarly) {
            body.gravityScale = movementParams.FallingGravity;
        }
        else {
            ApplyAdaptiveGravity();
        }
    }

    private void SelectState() {
        if (IsGrounded) {
            if (input.HorizontalMovementInput == 0 && Mathf.Abs(body.linearVelocityX) < 0.1f) {
                machine.Set(idleState);
            }
            else {
                machine.Set(walkState);
            }
        }
        else {
            if (IsMovingAgainstWall() && body.linearVelocityY <= 0) {
                machine.Set(wallSlideState);
            }
            else {
                machine.Set(airborneState);
            }
        }
    }

    private bool IsMovingAgainstWall() {
        int wallDirection = !IsTouchingWall ? 0 : IsTouchingLeftWall ? -1 : 1;
        return input.HorizontalMovementInput != 0 && input.HorizontalMovementInput == wallDirection;
    }

    #region Jump Methods
    private void OnJumpPressed() {
        queueJump = true;
        jumpBufferCounter = JumpBufferTime;
    }

    private void OnJumpReleased() {
        if (IsJumping && body.linearVelocityY > 0) {
            EndJumpEarly();
        }
    }

    private bool CanJump() {
        return IsGrounded || CanCoyoteJump();
    }

    private bool CanCoyoteJump() {
        return TimeSinceGrounded < movementParams.CoyoteTime && body.linearVelocityY <= 0;
    }

    private void HandleJumpInput() {
        if (queueJump && !IsCrowdControlled) {
            if (CanJump()) {
                Jump();
            }
            else if (IsTouchingWall) {
                WallJump();
            }
        }


    }
    private void Jump() {
        IsJumping = true;
        queueJump = false;
        JumpEndedEarly = false;
        body.linearVelocityY = movementParams.JumpSpeed;
    }
    private void EndJumpEarly() {
        JumpEndedEarly = true;
        body.linearVelocityY *= movementParams.JumpCutoffFactor;
    }

    private void UpdateJumpFlags() {
        if (IsGrounded && body.linearVelocityY <= 0) {
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

    private void WallJump() {
        int nearestWallDirection = IsTouchingLeftWall ? 1 : -1;
        body.linearVelocityX = nearestWallDirection * movementParams.HorizontalTopSpeed;
        StartCoroutine(DisableMovementForSeconds(0.3f));
        Jump();
    }
    private IEnumerator DisableMovementForSeconds(float time) {
        IsWallJumping = true;
        yield return new WaitForSeconds(time);
        IsWallJumping = false;
    }

    #endregion

    public void LoadPersistentData(GameData data) {
        body.position = data.playerPosition;
        body.linearVelocity = data.playerVelocity;
    }

    public void SavePersistentData(ref GameData data) {
        data.playerPosition = body.position;
        data.playerVelocity = body.linearVelocity;
    }

    public void SetDefaultPersistentData(ref GameData data) {
        data.playerPosition = startingPosition;
        data.playerVelocity = startingVelocity;
    }
}
