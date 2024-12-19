public class WallGrabState: State {

    public float hangTime;
    
    public override void Enter() {
    }

    public override void Do() {
        character.rigidBody.gravityScale = 0;
        character.rigidBody.linearVelocityY = 0;
        if(RunningTime > hangTime) {
            IsStateComplete = true;
        }
    }
}