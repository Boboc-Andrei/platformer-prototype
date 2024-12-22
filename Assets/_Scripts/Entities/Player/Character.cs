﻿using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour {
    [Header("Core Components")]
    public Rigidbody2D Body;
    public SpriteRenderer Sprite;
    public Animator Animator;
    
    [Header("Sensors")]
    public TerrainSensor GroundCheck;
    public TerrainSensor LeftWallCheck;
    public TerrainSensor RightWallCheck;

    [Header("Movement")]
    public CharacterMovementParameters MovementParams;
    public InputManager Input;

    [Header("Ledge grabbing")]
    public LedgeGrab LeftLedgeGrab;
    public LedgeGrab RightLedgeGrab;

    public StateMachine<Character> StateMachine = new StateMachine<Character>();

    // BLACKBOARD INFO
    public bool ApplyNormalGravityRules { get; set; } = true;
    public bool ApplyWalkingSpeedLimit { get; set; } = true;
    public bool DisableHorizontalMovement { get; set; } = false;
    public bool IsGrounded => GroundCheck.IsTouching;
    public float TimeSinceGrounded => GroundCheck.TimeSinceTouched;
    public int IsTouchingWall => LeftWallCheck.IsTouching ? -1 : RightWallCheck.IsTouching ? 1 : 0;
    public bool IsJumping { get; set; }
    public bool IsWallJumping { get; set; }
    public int WallJumpDirection { get; set; }
    public float HorizontalDrag { get; set; } = 0;


    #region Movement Methods
    public void ApplyAccelerationX(float acceleration) {
        Body.linearVelocityX += acceleration;
    }

    public void ApplyVelocityX(float velocity) {
        Body.linearVelocityX = velocity;
    }

    public void ApplyVelocityY(float velocity) {
        Body.linearVelocityY = velocity;
    }

    public void LookTowards(float directionX) {
        if (directionX != 0) {
            Sprite.flipX = directionX < 0;
        }
    }

    public void FaceMovementDirection() {
        if (Input.HorizontalMovement != 0) {
            LookTowards(Input.HorizontalMovement);
        }
        else if (Body.linearVelocityX > .1f) {
            LookTowards(Body.linearVelocityX);
        }
    }
    public void LimitWalkingSpeed() {
        Body.linearVelocityX = Mathf.Clamp(Body.linearVelocityX, -MovementParams.HorizontalTopSpeed, MovementParams.HorizontalTopSpeed);
    }

    public void ApplyHorizontalDrag() {
        if (Input.HorizontalMovement == 0) {
            Body.linearVelocityX *= HorizontalDrag;
        }
    }

    public void RoundHorizontalVelocityToZero() {
        if (Mathf.Abs(Body.linearVelocityX) < .1f) {
            Body.linearVelocityX = 0;
        }
    }
    #endregion

    #region Airborne methods
    public void ApplyJumpingGravity() {
        Body.gravityScale = MovementParams.RisingGravity;
    }

    public void ApplyFallingGravity() {
        Body.gravityScale = MovementParams.FallingGravity;
    }

    public void ApplyAdaptiveGravity() {
        if (!ApplyNormalGravityRules) return;
        if (Body.linearVelocityY > 0) {
            ApplyJumpingGravity();
        }
        else {
            ApplyFallingGravity();
        }
    }
    #endregion

    #region Jump Methods
    public virtual void Jump() {
        IsJumping = true;
        ApplyVelocityY(MovementParams.JumpSpeed);
    }

    public virtual bool CanJump() {
        return IsGrounded || CanCoyoteJump() && !IsJumping && Body.linearVelocityY <= 0;
    }

    public virtual bool CanCoyoteJump() {
        return TimeSinceGrounded <= MovementParams.CoyoteTime;
    }

    public virtual void WallJump() {
        IsJumping = true;
        IsWallJumping = true;
        WallJumpDirection = -IsTouchingWall;
        ApplyVelocityY(MovementParams.JumpSpeed);
        ApplyVelocityX(MovementParams.HorizontalTopSpeed * WallJumpDirection);
    }

    public virtual bool CanWallJump() {
        return IsTouchingWall != 0 && !IsGrounded && !IsJumping;
    }
    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        DrawStateGizmo();
    }

    private void DrawStateGizmo() {
        if (Application.isPlaying && StateMachine.CurrentState != null) {
            List<string> states = StateMachine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1, "State: " + string.Join(" > ", states));
        }
    }
#endif
}