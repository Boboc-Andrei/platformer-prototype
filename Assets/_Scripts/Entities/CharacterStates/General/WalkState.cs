using System.Collections;
using UnityEngine;

public class WalkState : State {
    new public AnimationClip animation;

    public override void Enter() {
        core.HorizontalDrag = movement.GroundHorizontalDrag;
        animator.Play(animation.name);
    }
    public override void Do() {
        animator.speed = Helpers.Map(Mathf.Abs(body.linearVelocityX), 0, core.movementParams.HorizontalTopSpeed, 0.2f, 1f);

        if (!core.IsGrounded || Mathf.Abs(body.linearVelocityX) < 0.1f) {
            IsStateComplete = true;
        }
    }

    public override void FixedDo() {

    }

    public override void Exit() {
        animator.speed = 1;
    }
}