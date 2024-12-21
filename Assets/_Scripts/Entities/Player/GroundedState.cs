using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGroundedState", menuName = "Scriptable Objects/Grounded State")]
internal class GroundedState : CharacterState {

    [SerializeField] private IdleState _idleStateSO;
    [SerializeField] private WalkingState _walkStateSO;

    private IdleState IdleState;
    private WalkingState WalkingState;
    public override void SetSubsatesBlackBoard() {
        IdleState = Instantiate(_idleStateSO);
        IdleState.SetBlackBoard(Character);

        WalkingState = Instantiate(_walkStateSO);
        WalkingState.SetBlackBoard(Character);
    }

    public override void Do() {
        if (!Character.IsGrounded) {
            IsComplete = true;
        }
        SelectSubState();
    }

    private void SelectSubState() {
        if (Character.Input.HorizontalMovement != 0 && Mathf.Abs(Character.Body.linearVelocityX) > .1f) {
            machine.Set(WalkingState);
        }
        else {
            machine.Set(IdleState);
        }
    }
}
