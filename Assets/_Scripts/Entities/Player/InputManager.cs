using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;

    public event System.Action OnJumpPressed;
    public event System.Action OnJumpReleased;

    public float HorizontalMovementInput;
    public float VerticalMovementInput;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        jumpAction.started += OnJumpActionStarted;
        jumpAction.canceled += OnJumpActionCanceled;
    }

    void Update() {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        //HorizontalMovementInput = movementInput.x;
        //VerticalMovementInput = movementInput.y;
    }

    private void OnJumpActionStarted(InputAction.CallbackContext obj) {
        OnJumpPressed?.Invoke();
    }

    private void OnJumpActionCanceled(InputAction.CallbackContext obj) {
        OnJumpReleased?.Invoke();
    }
}
