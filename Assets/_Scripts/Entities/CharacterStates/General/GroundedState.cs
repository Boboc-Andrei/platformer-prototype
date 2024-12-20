using UnityEngine;

public class GroundedState : State {
    public IdleState idleState;
    public WalkState walkState;
    public EmptyState emptyState;

    public override void Enter() {
        SetSubstate(idleState, true);
    }

    public override void Do() {
        character.RoundHorizontalVelocityToZero();
        SelectSubstate();
    }

    private void SelectSubstate() {
        if (!character.IsGrounded) {
            IsStateComplete = true;
        }
        if (character.input.HorizontalMovement != 0 || Mathf.Abs(character.rigidBody.linearVelocityX) > 0.1f) {
            SetSubstate(walkState);
        }
        else {
            SetSubstate(idleState);
        }
    }

    public override void FixedDo() {

    }

    public override void Exit() {

    }
}
