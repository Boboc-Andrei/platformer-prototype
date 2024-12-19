using NUnit.Framework.Constraints;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class StateMachine {

    public State CurrentState;

    public void Set(State newState, bool forceReset = false) {
        if (CurrentState != newState || forceReset) {
            string currentStateName = CurrentState == null ? "Null" : CurrentState.name;
            Debug.Log($"Transitioning from {currentStateName} to {newState?.name}");
            CurrentState?.ExitBranch();
            CurrentState = newState;
            CurrentState.Initialise(this);
            CurrentState.Enter();
        }
    }

    public List<string> GetActiveStateBranch(List<string> list = null) {
        if (list == null) {
            list = new List<string>();
        }

        if (CurrentState == null) {
            return list;
        }
        else {
            list.Add(CurrentState.name);
            return CurrentState.machine.GetActiveStateBranch(list);
        }

    }
}