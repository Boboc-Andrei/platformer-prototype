
using UnityEngine;

[CreateAssetMenu(fileName ="NewFallingState", menuName ="Scriptable Objects/FallingState")]
internal class FallingState : CharacterState {

    public override void Do() {
        if(Character.Body.linearVelocityY >= 0) {
            IsComplete = true;
        }
    }
    public override string ToString() {
        return "Falling";
    }
}