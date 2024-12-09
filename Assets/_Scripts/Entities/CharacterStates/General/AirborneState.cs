using System.Data;
using UnityEngine;

public class AirborneState : State {

    new public AnimationClip animation;
    public override void Enter() {
        core.HorizontalDrag = movement.AirHorizontalDrag;

        animator.speed = 0;
        animator.Play(animation.name);
    }
    public override void Do() {

        
        float time = Helpers.Map(body.linearVelocity.y, movement.JumpSpeed, -movement.TerminalVelocity, 0, 0.999f, true);
        animator.Play("Jump", 0, time);
        if(core.IsGrounded) {
            IsStateComplete = true;
        }
    }

    public override void FixedDo() {


    }

    public override void Exit() {
        animator.speed = 1;
    }
}
