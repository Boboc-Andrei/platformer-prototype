public class WallGrabState: State {

    public float hangTime;
    
    public override void Enter() {
        print("entered wall grab state");
    }

    public override void Do() {
        character.rigidBody.gravityScale = 0;
        character.rigidBody.linearVelocityY = 0;
        if(RunningTime > hangTime) {
            IsStateComplete = true;
        }
        print(IsStateComplete);
    }

    public override void Exit() {
        print("exiting wall grab state");
    }
}