

using UnityEngine;

public class BaseState <T> : IState{
    protected T blackBoard;
    public StateMachine<T> machine;
    public BaseState<T> CurrentSubState => machine.CurrentState;
    private float startTime;
    public float ElapsedTime => Time.time - startTime;

    public virtual void SetBlackBoard(T _blackBoard) {
        blackBoard = _blackBoard;
    }

    public virtual void Initialise() {
        startTime = Time.time;
    }

    public virtual void Do() { }
    public virtual void DoBranch() {
        Do();
        CurrentSubState?.Do();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }
    public virtual void ExitBranch() {
        CurrentSubState?.ExitBranch();
        Exit();
    }

    public virtual void FixedDo() { }
    public virtual void FixedDoBranch() {
        DoBranch();
        CurrentSubState?.DoBranch();
    }
}
