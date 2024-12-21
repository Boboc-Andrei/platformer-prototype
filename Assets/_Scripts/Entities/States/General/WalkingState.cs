using System;
using UnityEngine;
using UnityEngine.Diagnostics;

[CreateAssetMenu(fileName = "NewWalkingState", menuName = "Scriptable Objects/WalkingState")]
public class WalkingState : CharacterState {
    public AnimationClip Clip;

    public override void Enter() {
        Character.Animator.Play(Clip.name);
    }

    public override void Do() {
        if (Character.Input.HorizontalMovement == 0 && Character.Body.linearVelocityX < 0.1f) {
            IsComplete = true;
        }

        ModulateAnimatorSpeed();
    }

    private void ModulateAnimatorSpeed() {
        Character.Animator.speed = Helpers.Map(Mathf.Abs(Character.Body.linearVelocityX), 0, Character.MovementParams.HorizontalTopSpeed, 0, 0.999f, true);
    }
}