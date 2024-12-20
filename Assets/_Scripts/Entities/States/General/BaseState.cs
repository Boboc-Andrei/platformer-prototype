

using UnityEngine;

public abstract class BaseState <T> : ScriptableObject, IState {
    protected T Subject;
    public StateMachine<T> machine = new StateMachine<T>();
    public BaseState<T> CurrentSubState => machine.CurrentState;
    private float startTime;
    public float ElapsedTime => Time.time - startTime;


    public bool IsComplete { get; set; }

    public virtual void SetBlackBoard(T _subject) {
        Subject = _subject;
        IsComplete = false;
    }

    public virtual void Initialize() {
        startTime = Time.time;
    }

    public virtual void Do() { }
    public virtual void OnUpdate() {
        Do();
        CurrentSubState?.OnUpdate();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }
    public virtual void OnExit() {
        CurrentSubState?.OnExit();
        Exit();
    }
    public virtual void FixedDo() { }
    public virtual void OnFixedUpdate() {
        FixedDo();
        CurrentSubState?.OnFixedUpdate();
    }
}
