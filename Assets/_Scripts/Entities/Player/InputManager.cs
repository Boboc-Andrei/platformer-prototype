using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : CharacterController {
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction grabAction;

    private float JumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    void Start() {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        grabAction = InputSystem.actions.FindAction("Grab");

        jumpAction.started += OnJumpActionStarted;
        jumpAction.canceled += OnJumpActionCanceled;
    }

    void Update() {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        HorizontalMovement = movementInput.x;
        VerticalMovement = movementInput.y;

        Grab = grabAction.ReadValue<float>() > 0;

        jumpBufferCounter -= Time.deltaTime;
        if (jumpBufferCounter <= 0) {
            Jump = false;
        }

    }

    private void OnJumpActionStarted(InputAction.CallbackContext obj) {
        Jump = true;
        JumpConsumed = false;
        jumpBufferCounter = JumpBufferTime;
    }

    private void OnJumpActionCanceled(InputAction.CallbackContext obj) {
        StartCoroutine((EndJumpEarly()));
    }

    private IEnumerator EndJumpEarly() {
        CancelJump = true;
        yield return new WaitForEndOfFrame();
        CancelJump = false;
    }
}
