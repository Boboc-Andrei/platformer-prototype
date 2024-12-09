using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementParameters", menuName = "Scriptable Objects/CharacterMovementParameters")]
public class CharacterMovementParameters : ScriptableObject
{
    [Header("Horizontal Movement")]

    public float HorizontalTopSpeed = 3f;
    public float HorizontalAcceleration = 1f;

    [Header("Jumping")]
    public float JumpSpeed = 12f;
    public float JumpCutoffFactor = 0.5f;
    [Range(0, 0.5f)]public float CoyoteTime = 0.1f;

    [Header("Environment")]
    public float RisingGravity = 1f;
    public float FallingGravity = 2f;
    public float TerminalVelocity = 20f;
    [Range(0, 1)] public float GroundHorizontalDrag;
    [Range(0, 1)] public float AirHorizontalDrag;

    [Header("Wall Hanging")]
    [Range(0,1)] public float WallSlideDrag = 1f;
    public float WallSlideGravity = 1f;
    public float WallSlideMaximumVelocity = 2f;
    public float WallSlideMinimumVelocity = 1f;
}
