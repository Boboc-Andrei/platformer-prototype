using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterCore : MonoBehaviour {
    [Header("Core Components")]
    public Rigidbody2D body;
    public Animator animator;
    public StateMachine machine;
    public CharacterMovementParameters movementParams;
    public SpriteRenderer sprite;
    public State currentState => machine.CurrentState;

    [Header("Sensors")]
    public TerrainSensor groundCheck;
    public TerrainSensor leftWallCheck;
    public TerrainSensor rightWallCheck;

    [Header("Movement")]
    public float HorizontalDrag;
    public float HorizontalHeading;
    public bool IsCrowdControlled;
    public bool ApplyWalkingSpeedLimit = true;
    public bool FacingRight;
    public bool GravityOverride;

    public bool IsGrounded => groundCheck.IsTouching;
    public float TimeSinceGrounded => groundCheck.TimeSinceTouched;
    public bool IsTouchingLeftWall => leftWallCheck.IsTouching;
    public bool IsTouchingRightWall => rightWallCheck.IsTouching;
    public bool IsTouchingWall => leftWallCheck.IsTouching || rightWallCheck.IsTouching;


    protected void SetupStates() {
        machine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates) {
            state.SetCore(this);
        }
    }

    protected void RoundHorizontalVelocityToZero() {
        if (Mathf.Abs(body.linearVelocityX) < 0.1) {
            body.linearVelocityX = 0;
        }
    }

    protected void ApplyHorizontalFriction() {
        if (HorizontalHeading == 0) {
            body.linearVelocityX *= HorizontalDrag;
        }
    }

    protected void ApplyTerminalVelocity() {
        body.linearVelocityY = Mathf.Clamp(body.linearVelocityY, -movementParams.TerminalVelocity, movementParams.TerminalVelocity);
    }

    public void LimitWalkingSpeed() {
        if (ApplyWalkingSpeedLimit) {
            body.linearVelocityX = Mathf.Clamp(body.linearVelocityX, -movementParams.HorizontalTopSpeed, movementParams.HorizontalTopSpeed);
        }
    }

    protected void FaceMovementDirection() {
        if (IsCrowdControlled) return;

        if (HorizontalHeading != 0) {
            FacingRight = HorizontalHeading > 0;
        }
        else if (body.linearVelocityX != 0) {
            FacingRight = body.linearVelocityX > 0;
        }

        sprite.flipX = !FacingRight;
    }

    protected void ApplyAdaptiveGravity() {
        body.gravityScale = body.linearVelocityY > 0 ? movementParams.RisingGravity : movementParams.FallingGravity;
    }

    protected void HandleCoreMovement() {
        RoundHorizontalVelocityToZero();
        ApplyHorizontalFriction();
        ApplyTerminalVelocity();
        LimitWalkingSpeed();
        FaceMovementDirection();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        DrawStateGizmo();
    }

    private void DrawStateGizmo() {
        if (Application.isPlaying && currentState != null) {
            List<string> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1, "State: " + string.Join(" > ", states));
        }
    }
#endif
}
