
using Unity.Cinemachine;
using UnityEngine;

public class NavigateState : State {

    public Vector2 destination;
    public float threshold = 0.1f;
    public State walkState;

    public override void Enter() {
        SetSubstate(walkState, true);
    }

    public override void Do() {
        if (Mathf.Abs(core.transform.position.x - destination.x) < threshold) {
            core.HorizontalHeading = 0;
            IsStateComplete = true;
        }
        else {
            core.HorizontalHeading = Mathf.Sign(destination.x - body.position.x);
        }

    }

    public override void FixedDo() {
        body.linearVelocityX += movement.HorizontalAcceleration * core.HorizontalHeading;
    }
}