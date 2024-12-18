using UnityEngine;

public class WallFrictionSlideState : WallSlideState
{
    public override void Enter() {
        
    }

    public override void Do() {
        if(!character.IsTouchingWall || character.IsGrounded || character.rigidBody.linearVelocityY > 0) {
            IsStateComplete = true;
        }
    }

    public override void FixedDo() {
        character.rigidBody.linearVelocityY = character.movementParams.WallSlideMaximumVelocity == 0 ? 0 : Mathf.Max(character.rigidBody.linearVelocityY, -character.movementParams.WallSlideMaximumVelocity);
    }

    public override void Exit() {
        
    }
}
