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
    public bool ApplyWalkingSpeedLimit = true;
    public bool FacingRight;
    public bool GravityOverride;
    public bool DisableHorizontalMovement;
    public bool DisableHorizontalFacing;

    public bool IsGrounded => groundCheck.IsTouching;
    public float TimeSinceGrounded => groundCheck.TimeSinceTouched;
    public bool IsTouchingLeftWall => leftWallCheck.IsTouching;
    public bool IsTouchingRightWall => rightWallCheck.IsTouching;
    public bool IsTouchingWall => leftWallCheck.IsTouching || rightWallCheck.IsTouching;

    public bool IsGrabbingWall => IsTouchingWall && input.Grab;

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
        if (input.HorizontalMovement == 0) {
            rigidBody.linearVelocityX *= HorizontalDrag;
        }
    }

    public void ApplyTerminalVelocity() {
        rigidBody.linearVelocityY = Mathf.Clamp(rigidBody.linearVelocityY, -movementParams.TerminalVelocity, movementParams.TerminalVelocity);
    }

    public void LimitWalkingSpeed() {
        if (!ApplyWalkingSpeedLimit) return;
        rigidBody.linearVelocityX = Mathf.Clamp(rigidBody.linearVelocityX, -movementParams.HorizontalTopSpeed, movementParams.HorizontalTopSpeed);
    }

    public void FaceMovementDirection() {
        if (DisableHorizontalFacing) return;

        if (input.HorizontalMovement != 0) {
            FaceTowards(input.HorizontalMovement);
        }
        else if (rigidBody.linearVelocityX != 0) {
            FaceTowards(rigidBody.linearVelocityX);
        }

    }

    public void FaceTowards(float xDirection) {
        sprite.flipX = xDirection < 0;
    }

    public void ApplyAdaptiveGravity() {
        rigidBody.gravityScale = rigidBody.linearVelocityY > 0 ? movementParams.RisingGravity : movementParams.FallingGravity;
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


    
    public virtual bool CanJump() {
        return IsGrounded;
    }
#endif
}
