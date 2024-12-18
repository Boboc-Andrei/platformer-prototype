using UnityEngine;

public class GroundedState : State {
    public IdleState idleState;
    public WalkState walkState;

    public AnimationClip animationClip;
    public override void Enter() {
        character.animator.Play(animationClip.name);
    }

    public override void Do() {
        if (!character.IsGrounded) {
            IsStateComplete = true;
        }

        if (character.input.HorizontalMovementInput != 0 && Mathf.Abs(character.rigidBody.linearVelocityX) >= 0.1f) {
            SetSubstate(walkState);
        }
        else {
            SetSubstate(idleState);
        }


        character.RoundHorizontalVelocityToZero();
        character.ApplyHorizontalFriction();

        currentSubstate.DoBranch();
    }

    public override void FixedDo() {
        currentSubstate.FixedDoBranch();
    }

    public override void Exit() { }
}
