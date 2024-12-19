using System.ComponentModel;

public class JumpState : State{

    public WallJumpState wallJumpState;

    public override void Enter() {
        print("Entered jump state");
        if(character.IsTouchingWall) {
            SetSubstate(wallJumpState);
        }
    }

    public override void Do() {
        if(character.input.CancelJump) {
            character.rigidBody.linearVelocityY *= character.movementParams.JumpCutoffFactor;
            print("ending jump early");
            IsStateComplete = true;
        }
        if (character.rigidBody.linearVelocityY <= 0) {
            IsStateComplete = true;
        }
    }

    public override void Exit() {
        
    }
}