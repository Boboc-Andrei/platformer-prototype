﻿using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : Character {

    [Header("States")]
    [SerializeField] private GroundedState _groundedStateSO;
    private GroundedState GroundedState;

    [SerializeField] private AirborneState _airborneStateSO;
    private AirborneState AirborneState;

    [SerializeField] private OnWallState _wallStateSO;
    private OnWallState OnWallState;

    private void Start() {
        GroundedState = Instantiate(_groundedStateSO);
        GroundedState.SetBlackBoard(this);

        AirborneState = Instantiate(_airborneStateSO);
        AirborneState.SetBlackBoard(this);

        OnWallState = Instantiate(_wallStateSO);
        OnWallState.SetBlackBoard(this);

        StateMachine.Set(GroundedState);

        ApplyFallingGravity();
    }
    private void Update() {
        if (ApplyWalkingSpeedLimit) {
            LimitWalkingSpeed();
        }

        SelectState();

        StateMachine.CurrentState.OnUpdate();
    }
    private void FixedUpdate() {
        if (!DisableHorizontalMovement) {
            MoveWithInput();
        }
        JumpWithInput();


        LimitWalkingSpeed();
        ApplyHorizontalDrag();
        FaceMovementDirection();
        RoundHorizontalVelocityToZero();

        StateMachine.CurrentState.OnFixedUpdate();
    }
    private void SelectState() {

        if (StateMachine.CurrentState.IsComplete) {
            if (IsGrounded) {
                StateMachine.Set(GroundedState);
            }
            else {
                if (IsGrabbingWall != 0) {
                    StateMachine.Set(OnWallState, true);
                }
                else {
                    StateMachine.Set(AirborneState);
                }
            }
        }
    }

    private void MoveWithInput() {
        ApplyAccelerationX(Input.HorizontalMovement * MovementParams.HorizontalAcceleration);
    }


    private void JumpWithInput() {
        if (Input.Jump && !JumpingThroughPlatform) {
            if (Input.VerticalMovement < 0 && GroundCheck.IsTouchingLayer("Platforms") && Body.linearVelocityY == 0) {
                JumpDownFromPlatform();
            }
            else if (CanJump()) {
                Jump();
            }
            else if (CanWallJump()) {
                WallJump();
            }
        }
    }

    private void JumpDownFromPlatform() {
        ApplyVelocityY(4f);
        StartCoroutine(DisableCollisionForSeconds(.25f));
        StartCoroutine(DisableJumpingForSeconds(.35f));
    }
    private IEnumerator DisableCollisionForSeconds(float time) {
        HitBox.enabled = false;
        yield return new WaitForSeconds(time);
        HitBox.enabled = true;
    }

    private IEnumerator DisableJumpingForSeconds(float time) {
        IsJumpingDisabled = true;
        JumpingThroughPlatform = true;
        yield return new WaitForSeconds(time);
        IsJumpingDisabled = false;
        JumpingThroughPlatform = false;
    }
}