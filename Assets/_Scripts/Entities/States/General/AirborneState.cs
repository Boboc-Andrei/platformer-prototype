using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "AirborneState", menuName = "Scriptable Objects/AirborneState")]
public class AirborneState : CharacterState {
    public AnimationClip clip;

    [SerializeField] private JumpState _jumpStateSO;
    private JumpState JumpState;

    [SerializeField] private FallingState _fallingStateSO;
    private CharacterState FallingState;

    [SerializeField] private WallJumpState _wallJumpStateSO;
    private WallJumpState WallJumpState;

    public override void SetupSubStates() {
        JumpState = Instantiate(_jumpStateSO);
        JumpState.SetBlackBoard(Character);

        FallingState = Instantiate(_fallingStateSO);
        FallingState.SetBlackBoard(Character);

        WallJumpState = Instantiate(_wallJumpStateSO);
        WallJumpState.SetBlackBoard(Character);
    }

    public override void Enter() {
        Character.Animator.Play(clip.name);
        Character.Animator.speed = 0;
        Character.HorizontalDrag = Character.MovementParams.AirHorizontalDrag;
        SelectSubState();
    }

    public override void Do() {
        if ((Character.IsGrounded && !Character.IsJumping) || Character.IsGrabbingWall != 0) {
            IsComplete = true;
        }
        Character.ApplyAdaptiveGravity();
        MapVelocityToFrames();

        if(machine.CurrentState.IsComplete){
            SelectSubState();
        }
    }

    public override void Exit() {
        Character.Animator.speed = 1;
    }

    private void SelectSubState() {
        if (Character.IsJumping) {
            if (Character.IsWallJumping) {
                machine.Set(WallJumpState);
            }
            else {
                machine.Set(JumpState);
            }
        }
        else {
            machine.Set(FallingState);
        }
    }

    private void MapVelocityToFrames() {
        float time = Helpers.Map(Character.Body.linearVelocity.y, Character.MovementParams.JumpSpeed, -Character.MovementParams.TerminalVelocity, 0, 0.999f, true);
        Character.Animator.Play("Jump", 0, time);
    }

    public override string ToString() {
        return "Airborne";
    }
}
