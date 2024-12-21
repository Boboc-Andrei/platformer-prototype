using UnityEngine;

[CreateAssetMenu(fileName = "AirborneState", menuName = "Scriptable Objects/AirborneState")]
public class AirborneState : CharacterState {
    public AnimationClip clip;

    public override void Enter() {
        Character.Animator.Play(clip.name);
        Character.Animator.speed = 0;
    }

    public override void Do() {
        if(Character.IsGrounded) {
            IsComplete = true;
        }
    }

    public override void Exit() {
        Character.Animator.speed = 1;
    }

}
