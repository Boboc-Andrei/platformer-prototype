using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : Character {

    [SerializeField] private GroundedState _groundedStateSO;
    private GroundedState GroundedState;

    private void Start() {
        GroundedState = Instantiate(_groundedStateSO);
        GroundedState.SetBlackBoard(this);
        StateMachine.Set(GroundedState);
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
        LimitWalkingSpeed();
        ApplyHorizontalDrag();
        FaceMovementDirection();
        RoundHorizontalVelocityToZero();

        StateMachine.CurrentState.OnFixedUpdate();

    }

    private void SelectState() {
        StateMachine.Set(GroundedState);
    }

    private void MoveWithInput() {
        ApplyAccelerationX(Input.HorizontalMovement * MovementParams.HorizontalAcceleration);
    }
}