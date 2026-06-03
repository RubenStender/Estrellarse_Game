using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Jump : MonoBehaviour, IMovementModifier
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight       = 2.5f;
    [SerializeField] private float coyoteTime       = 0.12f;
    [SerializeField] private float jumpBufferTime   = 0.12f;
    [SerializeField] private int   maxJumps         = 1;     

    [Header("Variable Height")]
    [SerializeField] private float jumpCutMultiplier = 0.5f; 

    public bool IsActive => _isJumping;

 
    public void OnJumpPressed()  => _jumpBufferTimer = jumpBufferTime;
    public void OnJumpReleased() => _jumpReleased = true;

    private CharacterMotor _motor;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private int   _jumpsRemaining;
    private bool  _isJumping;
    private bool  _jumpReleased;
    private bool  _wasGrounded;

    private void Awake() => _motor = GetComponent<CharacterMotor>();

    private void Update()
    {
        bool grounded = _motor.IsGrounded;

        // Only reset on landing (air -> ground transition), not every frame while grounded
        if (grounded && !_wasGrounded)
        {
            _coyoteTimer = coyoteTime;
            _jumpsRemaining = maxJumps;
            _isJumping = false;
        }
        else if (!grounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }

        // Also reset if standing still on ground (for initial spawn/idle state)
        if (grounded && !_isJumping && _jumpsRemaining == 0)
        {
            _jumpsRemaining = maxJumps;
            _coyoteTimer = coyoteTime;
        }

        if (_jumpBufferTimer > 0f)
            _jumpBufferTimer -= Time.deltaTime;

        if (_jumpReleased && _isJumping && _motor.Velocity.y > 0f)
        {
            _motor.SetVerticalVelocity(_motor.Velocity.y * jumpCutMultiplier);
            _isJumping = false;
        }

        _jumpReleased = false;
        _wasGrounded = grounded;
    }

    public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
    {
        bool canJump = _coyoteTimer > 0f || _jumpsRemaining > 0;

        if (_jumpBufferTimer > 0f && canJump && !_isJumping) // <-- !_isJumping guard
        {
            _jumpBufferTimer = 0f;
            _coyoteTimer = 0f;
            _jumpsRemaining--;
            _isJumping = true;

            float jumpVelocity = Mathf.Sqrt(2f * 20f * jumpHeight);
            _motor.SetVerticalVelocity(jumpVelocity);
        }

        return Vector3.zero;
    }
}
