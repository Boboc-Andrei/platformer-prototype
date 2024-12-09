using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction grabAction;

    public event System.Action OnJumpPressed;
    public event System.Action OnJumpReleased;

    public float HorizontalMovementInput;
    public float VerticalMovementInput;
    public bool GrabPressed;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        grabAction = InputSystem.actions.FindAction("Grab");

        jumpAction.started += OnJumpActionStarted;
        jumpAction.canceled += OnJumpActionCanceled;

    }

    void Update() {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        HorizontalMovementInput = movementInput.x;
        VerticalMovementInput = movementInput.y;

        GrabPressed = grabAction.ReadValue<float>() > 0;
    }

    private void OnJumpActionStarted(InputAction.CallbackContext obj) {
        OnJumpPressed?.Invoke();
    }

    private void OnJumpActionCanceled(InputAction.CallbackContext obj) {
        OnJumpReleased?.Invoke();
    }
}
