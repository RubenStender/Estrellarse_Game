using UnityEngine;

<<<<<<< Updated upstream
/// <summary>
/// Handles horizontal walk/run movement.
/// Reads a movement direction set externally (by PlayerInputBridge or AI).
/// Supports speed tiers: walk, run, sprint.
/// </summary>
=======
>>>>>>> Stashed changes
[RequireComponent(typeof(CharacterMotor))]
public class WalkRun : MonoBehaviour, IMovementModifier
{
    [Header("Speeds")]
    [SerializeField] private float walkSpeed   = 4f;
    [SerializeField] private float runSpeed    = 7f;
    [SerializeField] private float sprintSpeed = 11f;

    [Header("Acceleration")]
    [SerializeField] private float acceleration    = 15f;
    [SerializeField] private float deceleration    = 20f;
<<<<<<< Updated upstream
    [SerializeField] private float airControlFactor = 0.4f; // Reduced control in air

    // Set by InputBridge or AI controller
    public Vector2 MoveInput { get; set; }   // Normalised WASD direction
    public bool IsSprinting  { get; set; }
    public bool IsWalking    { get; set; }   // Walk key held (slow)
=======
    [SerializeField] private float airControlFactor = 0.4f;

    public Vector2 MoveInput { get; set; } 
    public bool IsSprinting  { get; set; }
    public bool IsWalking    { get; set; }
>>>>>>> Stashed changes

    public bool IsActive => MoveInput.sqrMagnitude > 0.01f;

    private CharacterMotor _motor;
    private float _currentSpeed;

    private void Awake() => _motor = GetComponent<CharacterMotor>();

    public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
    {
        float targetSpeed = IsWalking ? walkSpeed : IsSprinting ? sprintSpeed : runSpeed;

        Vector3 wishDir = transform.right   * MoveInput.x
                        + transform.forward * MoveInput.y;
        wishDir.Normalize();

        float control = isGrounded ? 1f : airControlFactor;

        if (wishDir.sqrMagnitude > 0.01f)
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed,
                acceleration * control * Time.fixedDeltaTime);
        }
        else
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f,
                deceleration * control * Time.fixedDeltaTime);
        }

        Vector3 desiredHorizontal = wishDir * _currentSpeed;
        Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

<<<<<<< Updated upstream
        // Return only the delta needed to reach desired horizontal velocity
=======
>>>>>>> Stashed changes
        return desiredHorizontal - currentHorizontal;
    }

    public float GetCurrentSpeed() => _currentSpeed;
    public bool IsAtSprintSpeed() => IsSprinting && _currentSpeed > runSpeed * 0.9f;
}
