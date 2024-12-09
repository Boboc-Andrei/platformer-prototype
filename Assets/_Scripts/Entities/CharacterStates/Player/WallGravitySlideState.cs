using UnityEngine;

public class WallGravitySlideState: WallSlideState {
    new public AnimationClip animation;
    public override void Enter() {
        animator.Play(animation.name);
        core.GravityOverride = true;
        body.gravityScale = movement.WallSlideGravity;
    }

    public override void Do() {
        if (!core.IsTouchingWall || core.IsGrounded || body.linearVelocityY > 0) {
            IsStateComplete = true;
        }


        if(body.linearVelocityY < -movement.WallSlideMaximumVelocity) {
            body.linearVelocityY *= movement.WallSlideDrag;
        }
        else if(body.linearVelocityY > -movement.WallSlideMinimumVelocity) {
            body.linearVelocityY = -movement.WallSlideMinimumVelocity;
        }

    }

    public override void FixedDo() {


    }

    public override void Exit() {
        core.GravityOverride = false;
    }
}
