using System.Data;
using UnityEngine;

public class AirborneState : State {

    public JumpState jumpState;
    public WallJumpState wallJumpState;
    public FallingState fallingState;

    new public AnimationClip animation;
    public override void Enter() {
        character.HorizontalDrag = character.movementParams.AirHorizontalDrag;
        character.animator.speed = 0;
        character.animator.Play(animation.name);

        if (character.input.Jump) {
            if (character.IsWallJumping)
                SetSubstate(wallJumpState);
            else
                SetSubstate(jumpState);
        }
        else {
            SetSubstate(fallingState);
        }

    }
    public override void Do() {

        float time = Helpers.Map(character.rigidBody.linearVelocity.y, character.movementParams.JumpSpeed, -character.movementParams.TerminalVelocity, 0, 0.999f, true);
        character.animator.Play("Jump", 0, time);
        if (character.IsGrounded) {
            IsStateComplete = true;
        }

        if (currentSubstate.IsStateComplete) {
            if (currentSubstate == jumpState || currentSubstate == wallJumpState) {
                SetSubstate(fallingState);
            }
        }
        else if (character.IsTouchingWall && character.input.Jump && currentSubstate == fallingState) {
            SetSubstate(wallJumpState, true);
        }

        character.ApplyTerminalVelocity();
    }

    public override void FixedDo() {


    }

    public override void Exit() {
        character.animator.speed = 1;
    }
}
