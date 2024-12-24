
using UnityEngine;

[CreateAssetMenu(fileName ="NewFallingState", menuName ="Scriptable Objects/FallingState")]
internal class FallingState : CharacterState {

    public override void Do() {
        // TODO: find workaround for exiting falling state without using parameters belonging to other states
        if(Character.Body.linearVelocityY > 0 || Character.IsGrabbingWall != 0) {
            IsComplete = true;
        }
    }
    public override string ToString() {
        return "Falling";
    }
}