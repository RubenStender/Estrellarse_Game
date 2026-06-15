using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(WalkRun))]
public class Slide : MonoBehaviour, IMovementModifier
{
    [Header("Slide Settings")]
    [SerializeField] private float slideForce = 14f;
    [SerializeField] private float slideDuration = 0.6f;
    [SerializeField] private float slideFriction = 4f;
    [SerializeField] private float minSpeedToSlide = 6f;

    [Header("Slope Settings")]
    [SerializeField] private float slopeBoostMultiplier = 2.5f;
    [SerializeField] private float slopeCheckDistance = 1.2f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float normalHeight = 2f;

    [Header("Camera Crouch")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float standCameraY = 1.6f;
    [SerializeField] private float crouchCameraY = 0.8f;
    [SerializeField] private float lerpSpeed = 14f;

    [Header("Camera Tilt")]
    [SerializeField] private float slideTiltAngle = 8f;

    public bool IsSliding => _isSliding;
    public bool IsActive => _isSliding;
    public float CameraTiltTarget => _isSliding ? slideTiltAngle : 0f;

    private CharacterMotor _motor;
    private WalkRun _walkRun;
    private bool _isSliding;
    private Vector3 _slideVelocity;

    private void Awake()
    {
        _motor = GetComponent<CharacterMotor>();
        _walkRun = GetComponent<WalkRun>();
    }

    public void TrySlide()
    {
        if (_isSliding) return;
        if (!_motor.IsGrounded) return;
        if (_walkRun.GetCurrentSpeed() < minSpeedToSlide) return;
        StartCoroutine(SlideRoutine());
    }

    public void CancelSlide()
    {
        if (_isSliding) StopAllCoroutines();
        EndSlide();
    }

    private void Update()
    {
        if (cameraHolder == null) return;

        float targetY = _isSliding ? crouchCameraY : standCameraY;
        Vector3 pos = cameraHolder.localPosition;
        pos.y = Mathf.Lerp(pos.y, targetY, lerpSpeed * Time.deltaTime);
        cameraHolder.localPosition = pos;
    }

    public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
    {
        if (!_isSliding) return Vector3.zero;

        Vector3 groundNormal = GetGroundNormal();
        if (groundNormal != Vector3.up)
        {
            _slideVelocity = Vector3.ProjectOnPlane(_slideVelocity, groundNormal);
            Vector3 gravityAlongSlope = Vector3.ProjectOnPlane(Physics.gravity, groundNormal);
            _slideVelocity += gravityAlongSlope * Time.fixedDeltaTime;
        }

        _slideVelocity = Vector3.MoveTowards(_slideVelocity, Vector3.zero,
            slideFriction * Time.fixedDeltaTime);

        Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        return _slideVelocity - currentHorizontal;
    }

    private IEnumerator SlideRoutine()
    {
        _isSliding = true;
        _motor.Controller.height = crouchHeight;

        Vector3 groundNormal = GetGroundNormal();
        Vector3 slideDir = Vector3.ProjectOnPlane(transform.forward, groundNormal).normalized;
        float slopeDot = Vector3.Dot(groundNormal, Vector3.up);
        float slopeBoost = Mathf.Lerp(slopeBoostMultiplier, 1f, slopeDot);
        _slideVelocity = slideDir * (slideForce * slopeBoost);

        yield return new WaitForSeconds(slideDuration);
        EndSlide();
    }

    private void EndSlide()
    {
        _isSliding = false;
        _slideVelocity = Vector3.zero;
        _motor.Controller.height = normalHeight;
    }

    private Vector3 GetGroundNormal()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeCheckDistance))
            return hit.normal;
        return Vector3.up;
    }
}