using System.Collections;
using UnityEngine;

public abstract class State : MonoBehaviour {
    public bool IsStateComplete { get; protected set; }
    protected float StartTime;
    public float RunningTime => Time.time - StartTime;

    protected CharacterCore character;

    public StateMachine machine = new StateMachine();
    public StateMachine parent;
    public State currentSubstate => machine.CurrentState;

    protected void SetSubstate(State newState, bool forceReset = false) {
        machine.Set(newState, forceReset);
    }

    public virtual void Initialise(StateMachine _parent) {
        parent = _parent;
        IsStateComplete = false;
        StartTime = Time.time;
    }

    public void SetCore(CharacterCore _core) {
        character = _core;
    }
    public virtual void Enter() { }

    public virtual void Do() { }
    public virtual void DoBranch() {
        Do();
        currentSubstate?.DoBranch();
    }

    public virtual void FixedDo() { }
    public virtual void FixedDoBranch() {
        FixedDo();
        currentSubstate?.FixedDoBranch();
    }
    public virtual void Exit() { }
}