using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIdleState", menuName = "Scriptable Objects/IdleState")]
public class IdleState : CharacterState {

    public AnimationClip Clip;

    public override void Enter() {
        Character.Animator.Play(Clip.name);
    }

    public override void Do() {
        if (Character.Input.HorizontalMovement != 0 || !Character.IsGrounded) {
            IsComplete = true;
        }
    }
}