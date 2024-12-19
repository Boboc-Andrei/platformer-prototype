using UnityEngine;

public class WallSlideState : State
{
    public override void Enter() {
        character.rigidBody.gravityScale = character.movementParams.WallSlideGravity;
    }

    public override void Do() {
        character.rigidBody.linearVelocityY = Mathf.Clamp(character.rigidBody.linearVelocityY, -character.movementParams.WallSlideMaximumVelocity, 0);
    }
}
