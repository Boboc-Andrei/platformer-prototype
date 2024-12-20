using UnityEngine;

internal class OnWallState : State {

    public WallGrabState wallGrabState;
    public WallSlideState wallSlideState;

    public AnimationClip animationClip;

    private int wallDirection => character.IsTouchingLeftWall ? -1 : 1;

    public override void Enter() {
        character.GravityOverride = true;
        character.DisableHorizontalFacing = true;

        SetSubstate(wallGrabState, true);

        character.animator.Play(animationClip.name);
        character.FaceTowards(wallDirection);
    }

    public override void Do() {
        if(!character.IsGrabbingWall) {
            IsStateComplete = true;
        }

        if(currentSubstate.IsStateComplete) {
            if(currentSubstate == wallGrabState) {
                SetSubstate(wallSlideState);
            }
        }
    }


    public override void Exit() {
        print("exiting on wall state");
        character.GravityOverride = false;
        character.DisableHorizontalFacing = false;
    }
}