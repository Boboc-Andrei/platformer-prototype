using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : Character {

    [SerializeField] private IdleState _idleState;
    [SerializeField] private WalkingState _walkState;

    private IdleState IdleState;
    private WalkingState WalkingState;

    private void Start() {
        print(_idleState == null);

        IdleState = Instantiate(_idleState);
        IdleState.SetBlackBoard(this);

        WalkingState = Instantiate(_walkState);
        WalkingState.SetBlackBoard(this);

        StateMachine.Set(IdleState);
    }

    private void Update() {
        if (ApplyWalkingSpeedLimit) {
            LimitWalkingSpeed();
        }

        SelectState();

        StateMachine.CurrentState.OnUpdate();
    }

    private void SelectState() {
        if (Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > .1f) {
            StateMachine.Set(WalkingState);
        }
        else {
            StateMachine.Set(IdleState);
        }
    }

    private void FixedUpdate() {
        if (!DisableHorizontalMovement) {
            MoveWithInput();
        }
        LimitWalkingSpeed();
        ApplyHorizontalDrag();
        FaceMovementDirection();
        RoundHorizontalVelocityToZero();

        StateMachine.CurrentState.OnFixedUpdate();

    }

    private void MoveWithInput() {
        ApplyAccelerationX(Input.HorizontalMovement * MovementParams.HorizontalAcceleration);
    }
}