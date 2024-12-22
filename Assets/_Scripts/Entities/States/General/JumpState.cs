
using UnityEngine;

[CreateAssetMenu(fileName = "NewJumpState", menuName = "Scriptable Objects/JumpState")]
internal class JumpState : CharacterState {

    public override void Enter() {
        
    }

    public override void Do() {
        if(Character.Body.linearVelocityY < 0) {
            IsComplete = true;
        }
        else if(Character.Input.CancelJump) {
            Character.Body.linearVelocityY *= Character.MovementParams.JumpCutoffFactor;
            IsComplete = true;
        }
    }

    public override void Exit() {
        Character.IsJumping = false;
    }

    public override string ToString() {
        return "Jumping";
    }
}