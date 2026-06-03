using UnityEngine;

/// <summary>
/// Wall running. Detects walls to the left/right, sticks the player to them,
/// applies upward force to fight gravity, and signals CameraController to tilt.
/// Fully standalone — no wall run = don't add this component.
/// </summary>
[RequireComponent(typeof(CharacterMotor))]
public class WallRun : MonoBehaviour, IMovementModifier
{
    [Header("Detection")]
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField] private LayerMask wallMask = ~0;
    [SerializeField] private float minHeightAboveGround = 1.0f;

    [Header("Wall Run Feel")]
    [SerializeField] private float wallRunSpeed       = 9f;
    [SerializeField] private float wallRunUpForce     = 2f;   // Fights gravity
    [SerializeField] private float wallGravityScale   = 0.4f; // Reduced gravity on wall
    [SerializeField] private float maxWallRunTime     = 1.5f;
    [SerializeField] private float wallJumpForce      = 9f;
    [SerializeField] private float wallJumpUpForce    = 7f;

    [Header("Camera Tilt")]
    [SerializeField] private float cameraTiltAngle = 12f;

    public bool IsWallRunning     => _isWallRunning;
    public bool IsActive          => _isWallRunning;
    public float CameraTiltTarget => _isWallRunning ? (_onRightWall ? cameraTiltAngle : -cameraTiltAngle) : 0f;

    // Called by InputBridge
    public void TryWallJump()
    {
        if (!_isWallRunning) return;
        Vector3 wallJumpDir = (_wallNormal + Vector3.up).normalized;
        _motor.SetVerticalVelocity(wallJumpUpForce);
        _motor.AddImpulse(wallJumpDir * wallJumpForce);
        ExitWall();
    }

    private CharacterMotor _motor;
    private bool  _isWallRunning;
    private bool  _onRightWall;
    private float _wallRunTimer;
    private Vector3 _wallNormal;
    private RaycastHit _wallHit;

    private void Awake() => _motor = GetComponent<CharacterMotor>();

    private void Update()
    {
        CheckWalls();
    }

    private void CheckWalls()
    {
        bool rightWall = Physics.Raycast(transform.position, transform.right,
            out RaycastHit rightHit, wallCheckDistance, wallMask);
        bool leftWall  = Physics.Raycast(transform.position, -transform.right,
            out RaycastHit leftHit,  wallCheckDistance, wallMask);

        bool aboveGround = !Physics.Raycast(transform.position, Vector3.down,
            minHeightAboveGround, wallMask);

        if ((rightWall || leftWall) && !_motor.IsGrounded && aboveGround)
        {
            if (!_isWallRunning) EnterWall(rightWall, rightWall ? rightHit : leftHit);
            _onRightWall = rightWall;
            _wallNormal  = rightWall ? rightHit.normal : leftHit.normal;
            _wallHit     = rightWall ? rightHit : leftHit;
        }
        else
        {
            if (_isWallRunning) ExitWall();
        }
    }

    public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
    {
        if (!_isWallRunning) return Vector3.zero;

        _wallRunTimer -= Time.fixedDeltaTime;
        if (_wallRunTimer <= 0f) { ExitWall(); return Vector3.zero; }

        // Run along the wall surface
        Vector3 wallForward = Vector3.Cross(_wallNormal, Vector3.up);

        // Pick direction matching player's forward
        if (Vector3.Dot(wallForward, transform.forward) < 0f)
            wallForward = -wallForward;

        Vector3 desiredVelocity = wallForward * wallRunSpeed;

        // Fight gravity
        float desiredY = Mathf.Lerp(currentVelocity.y, wallRunUpForce, Time.fixedDeltaTime * 5f);
        desiredVelocity.y = desiredY;

        return (desiredVelocity - currentVelocity) * Time.fixedDeltaTime * 10f;
    }

    private void EnterWall(bool right, RaycastHit hit)
    {
        _isWallRunning = true;
        _wallRunTimer  = maxWallRunTime;
        _onRightWall   = right;
        _wallNormal    = hit.normal;
    }

    private void ExitWall()
    {
        _isWallRunning = false;
    }
}
