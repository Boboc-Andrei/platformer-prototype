
using UnityEngine;

public class NPC : CharacterCore {

    public PatrolState patrolState;

    private void Start() {
        SetupStates();
        machine.Set(patrolState);
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
