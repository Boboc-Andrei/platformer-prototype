
using Unity.Cinemachine;
using UnityEngine;

public class NavigateState : State {

    public Vector2 destination;
    public float threshold = 0.1f;
    public State walkState;

    public override void Enter() {
        SetSubstate(walkState, true);
    }

    public override void Do() {
        if (Mathf.Abs(character.transform.position.x - destination.x) < threshold) {
            character.input.HorizontalMovement = 0;
            IsStateComplete = true;
        }
        else {
            character.input.HorizontalMovement = Mathf.Sign(destination.x - character.rigidBody.position.x);
        }

    }

    public override void FixedDo() {
        character.rigidBody.linearVelocityX += character.movementParams.HorizontalAcceleration * character.input.HorizontalMovement;
    }
}