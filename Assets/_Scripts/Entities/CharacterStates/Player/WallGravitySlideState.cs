using UnityEngine;

public class WallGravitySlideState: WallSlideState {
    new public AnimationClip animation;
    public override void Enter() {
        character.animator.Play(animation.name);
        character.GravityOverride = true;
        character.rigidBody.gravityScale = character.movementParams.WallSlideGravity;
    }

    public override void Do() {
        if (!character.IsTouchingWall || character.IsGrounded || character.rigidBody.linearVelocityY > 0) {
            IsStateComplete = true;
        }


        if(character.rigidBody.linearVelocityY < -character.movementParams.WallSlideMaximumVelocity) {
            character.rigidBody.linearVelocityY *= character.movementParams.WallSlideDrag;
        }
        else if(character.rigidBody.linearVelocityY > -character.movementParams.WallSlideMinimumVelocity) {
            character.rigidBody.linearVelocityY = -character.movementParams.WallSlideMinimumVelocity;
        }

    }

    public override void FixedDo() {


    }

    public override void Exit() {
        character.GravityOverride = false;
    }
}
