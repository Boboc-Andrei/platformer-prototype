using UnityEngine;
public interface ICharacterController {
    public bool Jump {  get; set; }
    public bool CancelJump { get; set; }
    public float HorizontalMovement { get; set; }
    public float VerticalMovement { get; set; }
    public bool Grab { get; set; }
}

public abstract class CharacterController : MonoBehaviour, ICharacterController {
    public bool Jump { get; set; }
    public bool CancelJump { get; set; }
    public float HorizontalMovement { get; set; }
    public float VerticalMovement { get; set; }
    public bool Grab { get; set; }
}