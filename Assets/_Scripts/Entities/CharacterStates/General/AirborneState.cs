using System.Data;
using UnityEngine;

public class AirborneState : State {

    public JumpState jumpState;

    new public AnimationClip animation;
    public override void Enter() {
        character.HorizontalDrag = character.movementParams.AirHorizontalDrag;
        character.animator.speed = 0;
        character.animator.Play(animation.name);
        print($"entered airborne state. jump input pressed: {character.input.Jump}");
        if(character.input.Jump) {
            SetSubstate(jumpState);
        }
    }
    public override void Do() {

        float time = Helpers.Map(character.rigidBody.linearVelocity.y, character.movementParams.JumpSpeed, -character.movementParams.TerminalVelocity, 0, 0.999f, true);
        character.animator.Play("Jump", 0, time);
        if(character.IsGrounded) {
            IsStateComplete = true;
        }


    }

    public override void FixedDo() {


    }

    public override void Exit() {
        character.animator.speed = 1;
    }
}
