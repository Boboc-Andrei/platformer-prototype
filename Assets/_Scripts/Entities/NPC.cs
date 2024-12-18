
using UnityEngine;

public class NPC : CharacterCore {

    public PatrolState patrolState;

    private void Start() {
        SetupStates();
        stateMachine.Set(patrolState);
    }

    private void Update() {
        if (currentState.IsStateComplete) {

        }
        currentState.DoBranch();
    }

    private void FixedUpdate() {
        HandleCoreMovement();
        if (currentState.IsStateComplete) {

        }
        currentState.FixedDoBranch();
    }
}
