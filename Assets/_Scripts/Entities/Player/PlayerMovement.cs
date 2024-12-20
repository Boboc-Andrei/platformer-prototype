using System.Collections;
using UnityEngine;

public class PlayerMovement : Character {


    private void Awake() {


    }

    private void Update() {

        if (!DisableHorizontalMovement) {
            MoveWithInput();
        }
        if (ApplyWalkingSpeedLimit) {
            LimitWalkingSpeed();
        }

    }

    private void FixedUpdate() {

    }

    private void MoveWithInput() {
        ApplyAccelerationX(Input.HorizontalMovement * MovementParams.HorizontalAcceleration);
    }
}