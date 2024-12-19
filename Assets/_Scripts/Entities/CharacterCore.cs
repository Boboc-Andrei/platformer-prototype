using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterCore : MonoBehaviour {
    [Header("Core Components")]
    public Rigidbody2D rigidBody;
    public Animator animator;
    public StateMachine stateMachine;
    public CharacterMovementParameters movementParams;
    public SpriteRenderer sprite;
    public CharacterController input;
    public State currentState => stateMachine.CurrentState;

    [Header("Sensors")]
    public TerrainSensor groundCheck;
    public TerrainSensor leftWallCheck;
    public TerrainSensor rightWallCheck;

    [Header("Movement")]
    public float HorizontalDrag;
    public virtual float HorizontalInput { get; set; }
    public bool ApplyWalkingSpeedLimit = true;
    public bool FacingRight;
    public bool GravityOverride;

    public bool IsGrounded => groundCheck.IsTouching;
    public float TimeSinceGrounded => groundCheck.TimeSinceTouched;
    public bool IsTouchingLeftWall => leftWallCheck.IsTouching;
    public bool IsTouchingRightWall => rightWallCheck.IsTouching;
    public bool IsTouchingWall => leftWallCheck.IsTouching || rightWallCheck.IsTouching;


    protected void SetupStates() {
        stateMachine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates) {
            state.SetCore(this);
        }
    }

    public void RoundHorizontalVelocityToZero() {
        if (Mathf.Abs(rigidBody.linearVelocityX) < 0.1) {
            rigidBody.linearVelocityX = 0;
        }
    }

    public void ApplyHorizontalFriction() {
        if (HorizontalInput == 0) {
            rigidBody.linearVelocityX *= HorizontalDrag;
        }
    }

    public void ApplyTerminalVelocity() {
        rigidBody.linearVelocityY = Mathf.Clamp(rigidBody.linearVelocityY, -movementParams.TerminalVelocity, movementParams.TerminalVelocity);
    }

    public void LimitWalkingSpeed() {
        if (ApplyWalkingSpeedLimit) {
            rigidBody.linearVelocityX = Mathf.Clamp(rigidBody.linearVelocityX, -movementParams.HorizontalTopSpeed, movementParams.HorizontalTopSpeed);
        }
    }

    public void FaceMovementDirection() {
        if (HorizontalInput != 0) {
            FacingRight = HorizontalInput > 0;
        }
        else if (rigidBody.linearVelocityX != 0) {
            FacingRight = rigidBody.linearVelocityX > 0;
        }

        sprite.flipX = !FacingRight;
    }

    public void ApplyAdaptiveGravity() {
        rigidBody.gravityScale = rigidBody.linearVelocityY > 0 ? movementParams.RisingGravity : movementParams.FallingGravity;
    }

    protected void HandleCoreMovement() {
        ApplyTerminalVelocity(); // MOVE TO AIRBORNE STATE
        LimitWalkingSpeed(); // MOVE TO WALK STATE
        FaceMovementDirection(); // MOVE TO ... ? let cores call this manually
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        DrawStateGizmo();
    }

    private void DrawStateGizmo() {
        if (Application.isPlaying && currentState != null) {
            List<string> states = stateMachine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1, "State: " + string.Join(" > ", states));
        }
    }
#endif
}
