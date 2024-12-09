using UnityEngine;

public class WallFrictionSlideState : WallSlideState
{
    public override void Enter() {
        
    }

    public override void Do() {
        if(!core.IsTouchingWall || core.IsGrounded || body.linearVelocityY > 0) {
            IsStateComplete = true;
        }
    }

    public override void FixedDo() {
        body.linearVelocityY = core.movementParams.WallSlideMaximumVelocity == 0 ? 0 : Mathf.Max(body.linearVelocityY, -core.movementParams.WallSlideMaximumVelocity);
    }

    public override void Exit() {
        
    }
}
