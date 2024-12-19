using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class WallJumpState : JumpState {

    public float movementDisableDuration = 0.2f;
    private float wallDirection;
    public override void Enter() {
        base.Enter();

        wallDirection = character.IsTouchingLeftWall ? 1 : -1;
        character.rigidBody.linearVelocityX = wallDirection * character.movementParams.HorizontalTopSpeed;
        character.DisableHorizontalMovement = true;
    }

    public override void Do() {
        base.Do();
        if (RunningTime > movementDisableDuration) {
            IsStateComplete = true;
        }
        character.input.HorizontalMovement = -wallDirection;
    }

    public override void Exit() {
        base.Exit();
        character.DisableHorizontalMovement = false;
    }
}