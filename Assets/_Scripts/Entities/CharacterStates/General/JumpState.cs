using System.ComponentModel;

public class JumpState : State{


    public override void Enter() {
        character.ApplyJumpingGravity();
    }

    public override void Do() {
        if (IsStateComplete) return;

        if(character.input.CancelJump) {
            character.rigidBody.linearVelocityY *= character.movementParams.JumpCutoffFactor;
            character.rigidBody.gravityScale = character.movementParams.FallingGravity;
            IsStateComplete = true;
        }
        if (character.rigidBody.linearVelocityY <= 0) {
            IsStateComplete = true;
        }

    }

    public override void Exit() {
        character.IsJumping = false;
    }
}