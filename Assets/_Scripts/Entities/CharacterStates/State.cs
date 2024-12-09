using System.Collections;
using UnityEngine;

public abstract class State : MonoBehaviour {
    public bool IsStateComplete { get; protected set; }
    protected float StartTime;
    public float RunningTime => Time.time - StartTime;

    protected CharacterCore core;
    protected Rigidbody2D body => core.body;
    protected Animator animator => core.animator;
    protected CharacterMovementParameters movement => core.movementParams;

    public StateMachine machine = new StateMachine();
    public StateMachine parent;
    public State state => machine.CurrentState;

    protected void SetSubstate(State newState, bool forceReset = false) {
        machine.Set(newState, forceReset);
    }

    public virtual void Initialise(StateMachine _parent) {
        parent = _parent;
        IsStateComplete = false;
        StartTime = Time.time;
    }

    public void SetCore(CharacterCore _core) {
        core = _core;
    }
    public virtual void Enter() { }

    public virtual void DoBranch() {
        Do();
        state?.DoBranch();
    }

    public virtual void Do() { }
    public virtual void FixedDo() { }
    public virtual void FixedDoBranch() {
        FixedDo();
        state?.FixedDoBranch();
    }
    public virtual void Exit() { }
}