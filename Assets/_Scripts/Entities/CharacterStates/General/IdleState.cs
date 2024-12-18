using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {

    new public AnimationClip animation;
    public List<AnimationClip> secondaryAnimations = new List<AnimationClip>();
    public float secondaryAnimationFrequency = 15;
    private float timeElapsed;
    private bool isPlayingSecondaryAnimation;
    public override void Enter() {
        character.animator.Play(animation.name);

    }
    public override void Do() {
        if (!character.IsGrounded || Mathf.Abs(character.rigidBody.linearVelocityX) > 0.1) {
            IsStateComplete = true;
        }

        timeElapsed += Time.deltaTime;
        if (secondaryAnimations.Count > 0 && timeElapsed > secondaryAnimationFrequency) {
            if (!isPlayingSecondaryAnimation) {
                PlayRandomAnimation();
            }
            else if (IsAnimationCompleted()) {
                PlayDefaultAnimation();
            }
        }
    }

    private void PlayRandomAnimation() {
        AnimationClip clip = ChooseRandomAnimation();
        if (clip != null) {
            character.animator.Play(clip.name);
            isPlayingSecondaryAnimation = true;
        }
    }

    private void PlayDefaultAnimation() {
        isPlayingSecondaryAnimation = false;
        character.animator.Play(animation.name);
        timeElapsed = 0;
    }

    private AnimationClip ChooseRandomAnimation() {
        return secondaryAnimations[Random.Range(0, secondaryAnimations.Count)];
    }

    private bool IsAnimationCompleted() {
        return character.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }

    public override void FixedDo() {

    }

    public override void Exit() {

    }
}