using UnityEngine;
[RequireComponent(typeof(CharacterMotor))]
public class WallRun : MonoBehaviour, IMovementModifier
{
    [Header("Detection")]
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField] private LayerMask wallMask = ~0;
    [SerializeField] private float minHeightAboveGround = 1.0f;
    [Header("Wall Run Feel")]
    [SerializeField] private float wallRunSpeed = 9f;
    [SerializeField] private float wallRunUpForce = 2f;
    [SerializeField] private float wallGravityScale = 0.4f;
    [SerializeField] private float maxWallRunTime = 1.5f;
    [Header("Wall Jump")]
    [SerializeField] private float wallJumpUpForce = 7f;
    [SerializeField] private float wallJumpSideForce = 7f;
    [SerializeField] private float wallJumpForwardForce = 5f;
    [Header("Camera Tilt")]
    [SerializeField] private float cameraTiltAngle = 12f;

    public bool IsWallRunning => _isWallRunning;
    public bool IsActive => _isWallRunning;
    public float CameraTiltTarget => _isWallRunning ? (_onRightWall ? cameraTiltAngle : -cameraTiltAngle) : 0f;

    private CharacterMotor _motor;
    private bool _isWallRunning;
    private bool _onRightWall;
    private float _wallRunTimer;
    private float _wallExitCooldown;
    private Vector3 _wallNormal;
    private RaycastHit _wallHit;

    private void Awake() => _motor = GetComponent<CharacterMotor>();

    private void Update()
    {
        if (_wallExitCooldown > 0f) _wallExitCooldown -= Time.deltaTime;
        CheckWalls();
    }

    public void TryWallJump()
    {
        if (!_isWallRunning) return;

        Vector3 wallForward = Vector3.Cross(_wallNormal, Vector3.up);
        if (Vector3.Dot(wallForward, transform.forward) < 0f)
            wallForward = -wallForward;

        _wallExitCooldown = 0.5f;
        _motor.ZeroHorizontalVelocity();
        _motor.SetVerticalVelocity(wallJumpUpForce);
        _motor.AddImpulse(_wallNormal * wallJumpSideForce + wallForward * wallJumpForwardForce);
        ExitWall();
    }

    private void CheckWalls()
    {
        bool rightWall = Physics.Raycast(transform.position, transform.right,
            out RaycastHit rightHit, wallCheckDistance, wallMask);
        bool leftWall = Physics.Raycast(transform.position, -transform.right,
            out RaycastHit leftHit, wallCheckDistance, wallMask);
        bool aboveGround = !Physics.Raycast(transform.position, Vector3.down,
            minHeightAboveGround, wallMask);

        if ((rightWall || leftWall) && !_motor.IsGrounded && aboveGround && _wallExitCooldown <= 0f)
        {
            if (!_isWallRunning) EnterWall(rightWall, rightWall ? rightHit : leftHit);
            _onRightWall = rightWall;
            _wallNormal = rightWall ? rightHit.normal : leftHit.normal;
            _wallHit = rightWall ? rightHit : leftHit;
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

        Vector3 wallForward = Vector3.Cross(_wallNormal, Vector3.up);
        if (Vector3.Dot(wallForward, transform.forward) < 0f)
            wallForward = -wallForward;

        Vector3 desiredVelocity = wallForward * wallRunSpeed;
        float desiredY = Mathf.Lerp(currentVelocity.y, wallRunUpForce, Time.fixedDeltaTime * 5f);
        desiredVelocity.y = desiredY;
        return (desiredVelocity - currentVelocity) * Time.fixedDeltaTime * 10f;
    }

    private void EnterWall(bool right, RaycastHit hit)
    {
        _isWallRunning = true;
        _wallRunTimer = maxWallRunTime;
        _onRightWall = right;
        _wallNormal = hit.normal;
    }

    private void ExitWall() => _isWallRunning = false;
}