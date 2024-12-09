using UnityEngine;


public class PatrolState : State {
    public NavigateState navigate;
    public IdleState idle;

    public Transform point1;
    public Transform point2;
    public int idleTime = 10;

    protected void GoToNextDestination() {
        float randomSpot = Random.Range(point1.position.x, point2.position.x);
        navigate.destination = new Vector2(randomSpot, core.transform.position.y);
        SetSubstate(navigate, true);
    }

    public override void Enter() {
        GoToNextDestination();
    }

    public override void Do() {
        if (machine.CurrentState == navigate) {
            if (navigate.IsStateComplete) {
                SetSubstate(idle, true);
            }
        }
        else {
            if(machine.CurrentState.RunningTime > idleTime) {
                GoToNextDestination();
            }
        }
    }
}
