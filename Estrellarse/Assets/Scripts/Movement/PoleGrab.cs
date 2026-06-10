using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PoleGrab : MonoBehaviour, IMovementModifier
{
    [Header("Detection")]
    [SerializeField] private float grabRadius = 1.2f;
    [SerializeField] private LayerMask poleMask = ~0;

    [Header("Swing Feel")]
    [SerializeField] private float swingSpeed = 3f;
    [SerializeField] private float maxSwingAngle = 70f;
    [SerializeField] private float swingGravity = -15f;

    [Header("Jump Off")]
    [SerializeField] private float jumpUpForce = 8f;
    [SerializeField] private float jumpForwardForce = 6f;
    [SerializeField] private float releaseExitCooldown = 0.4f;

    [Header("Camera Tilt")]
    [SerializeField] private float cameraTiltAngle = 6f;

    public bool IsActive => _isHanging;
    public float CameraTiltTarget => _isHanging ? cameraTiltAngle : 0f;

    private CharacterMotor _motor;
    private bool _isHanging;
    private float _exitCooldown;
    private Vector3 _polePoint;
    private float _swingAngle;
    private float _swingVelocity;

    private void Awake()
    {
        _motor = GetComponent<CharacterMotor>();
        _motor.RefreshModifiers();
    }

    private void Update()
    {
        if (_exitCooldown > 0f) _exitCooldown -= Time.deltaTime;
        if (!_isHanging) TryGrab();
    }

    public void TryJumpOff()
    {
        if (!_isHanging) return;

        _exitCooldown = releaseExitCooldown;
        _motor.ZeroHorizontalVelocity();
        _motor.SetVerticalVelocity(jumpUpForce);
        _motor.AddImpulse(transform.forward * jumpForwardForce);
        ExitPole();
    }

    private void TryGrab()
    {
        if (_exitCooldown > 0f) return;
        if (_motor.IsGrounded) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, grabRadius, poleMask);
        if (hits.Length == 0) return;

        Collider pole = hits[0];
        _polePoint = pole.ClosestPoint(transform.position);

        EnterPole();
    }

    private void EnterPole()
    {
        _isHanging = true;
        _swingAngle = 0f;
        _swingVelocity = 0f;
        _motor.GravityScale = 0f;
        _motor.ZeroHorizontalVelocity();
        _motor.SetVerticalVelocity(0f);
    }

    private void ExitPole()
    {
        _isHanging = false;
        _motor.GravityScale = 1f;
    }

    public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
    {
        if (!_isHanging) return Vector3.zero;
        if (isGrounded) { ExitPole(); return Vector3.zero; }

        float ropeLength = Mathf.Max(0.1f, _polePoint.y - transform.position.y);

        float angularAccel = (swingGravity / ropeLength) * Mathf.Sin(_swingAngle * Mathf.Deg2Rad);
        _swingVelocity += angularAccel * Time.fixedDeltaTime * swingSpeed;
        _swingVelocity = Mathf.Clamp(_swingVelocity, -maxSwingAngle, maxSwingAngle);
        _swingAngle += _swingVelocity * Time.fixedDeltaTime;
        _swingAngle = Mathf.Clamp(_swingAngle, -maxSwingAngle, maxSwingAngle);

        Vector3 desiredOffset = new Vector3(
            Mathf.Sin(_swingAngle * Mathf.Deg2Rad) * ropeLength,
            -Mathf.Cos(_swingAngle * Mathf.Deg2Rad) * ropeLength,
            0f
        );

        Vector3 desiredPos = _polePoint + desiredOffset;
        Vector3 desiredVelocity = (desiredPos - transform.position) / Time.fixedDeltaTime;
        desiredVelocity.y = Mathf.Clamp(desiredVelocity.y, -10f, 10f);

        return (desiredVelocity - currentVelocity) * Time.fixedDeltaTime * 8f;
    }
}