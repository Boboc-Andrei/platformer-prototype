using UnityEngine;

public class GroundedState : State {
    public IdleState idleState;
    public WalkState walkState;
    public EmptyState emptyState;

    public override void Enter() {
        print("entered grounded state");
        SetSubstate(idleState, true);
    }

    public override void Do() {
        if (!character.IsGrounded) {
            IsStateComplete = true;
        }
        if (character.input.HorizontalMovement == 0 && Mathf.Abs(character.rigidBody.linearVelocityX) < 0.1f) {
            SetSubstate(idleState);
        }
        else {
            SetSubstate(walkState);
        }

        character.RoundHorizontalVelocityToZero();
    }


    public override void FixedDo() {

    }

    public override void Exit() {
        print("leaving grounded state");
    }
}
