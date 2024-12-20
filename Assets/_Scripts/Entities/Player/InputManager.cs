using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, ICharacterInput {
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction grabAction;

    private float JumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public float HorizontalMovement { get; set; }
    public float VerticalMovement { get; set; }
    public bool Jump { get; set; }
    public bool HoldJump { get; set; }
    public bool Grab { get; set; }
    public bool CancelJump { get; set; }

    void Start() {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        grabAction = InputSystem.actions.FindAction("Grab");

        jumpAction.started += OnJumpPressed;
        jumpAction.canceled += OnJumpReleased;
    }

    void Update() {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        HorizontalMovement = movementInput.x;
        VerticalMovement = movementInput.y;
        Grab = grabAction.ReadValue<float>() > 0;
        HoldJump = jumpAction.ReadValue<float>() > 0;

        jumpBufferCounter -= Time.deltaTime;
        if (jumpBufferCounter <= 0) {
            Jump = false;
        }

    }

    private void OnJumpPressed(InputAction.CallbackContext obj) {
        Jump = true;
        jumpBufferCounter = JumpBufferTime;
    }

    private void OnJumpReleased(InputAction.CallbackContext obj) {
        StartCoroutine((EndJumpEarly()));
    }

    private IEnumerator EndJumpEarly() {

        CancelJump = true;
        yield return new WaitForEndOfFrame();
        CancelJump = false;
    }
}

public interface ICharacterInput {
    public float HorizontalMovement { get; set; }
    public float VerticalMovement { get; set; }
    public bool Jump { get; set; }
    public bool CancelJump { get; set; }
    public bool Grab { get; set; }
    public bool HoldJump { get; set; }
}
