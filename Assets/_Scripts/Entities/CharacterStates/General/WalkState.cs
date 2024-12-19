using System.Collections;
using UnityEngine;

public class WalkState : State {
    new public AnimationClip animation;

    public override void Enter() {
        character.HorizontalDrag = character.movementParams.GroundHorizontalDrag;
        print("entered walk state. playing walk animation");
        character.animator.Play(animation.name);
    }
    public override void Do() {
        if (!character.IsGrounded || Mathf.Abs(character.rigidBody.linearVelocityX) < 0.1f) {
            IsStateComplete = true;
        }

        ModulateAnimatorSpeed();
    }

    public override void FixedDo() {

    }

    public override void Exit() {
        print("leaving walking state");
        character.animator.speed = 1;
    }

    private void ModulateAnimatorSpeed() {
        character.animator.speed = Helpers.Map(Mathf.Abs(character.rigidBody.linearVelocityX), 0, character.movementParams.HorizontalTopSpeed, 0.2f, 1f);

    }
}