using UnityEngine;

public class LedgeGrabState : State {

    new public AnimationClip animation;

    public override void Enter() {
        character.animator.Play(animation.name);
        character.GravityOverride = true;
        character.rigidBody.gravityScale = 0f;
        character.rigidBody.linearVelocity = Vector2.zero;
    }

    public override void Do() {
        print("grabbing ledge");
    }

    public override void Exit() {
        character.GravityOverride = false;
        character.rigidBody.gravityScale = character.movementParams.FallingGravity;
    }
}