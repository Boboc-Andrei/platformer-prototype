using UnityEngine;

public class LedgeGrabState : State {

    new public AnimationClip animation;

    public override void Enter() {
        animator.Play(animation.name);
        core.GravityOverride = true;
        core.body.gravityScale = 0f;
        core.body.linearVelocity = Vector2.zero;
    }

    public override void Do() {
        print("grabbing ledge");
    }

    public override void Exit() {
        core.GravityOverride = false;
        core.body.gravityScale = core.movementParams.FallingGravity;
    }
}