using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementParameters", menuName = "Scriptable Objects/CharacterMovementParameters")]
public class CharacterMovementParameters : ScriptableObject
{
    [Header("Horizontal Movement")]

    public float HorizontalTopSpeed = 9f;
    public float HorizontalAcceleration = .75f;

    [Header("Jumping")]
    public float JumpSpeed = 21f;
    public float JumpCutoffFactor = 0.5f;
    [Range(0, 0.5f)]public float CoyoteTime = 0.15f;

    [Header("Environment")]
    public float RisingGravity = 4f;
    public float FallingGravity = 6f;
    public float TerminalVelocity = 30f;
    [Range(0, 1)] public float GroundHorizontalDrag = .8f;
    [Range(0, 1)] public float AirHorizontalDrag = .95f;

    [Header("Wall Hanging")]
    public float WallSlideGravity = 1f;
    public float WallSlideMaximumVelocity = 2f;
}
