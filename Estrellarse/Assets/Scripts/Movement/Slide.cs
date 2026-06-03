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

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float normalHeight = 2f;

    [Header("Camera Crouch")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float standCameraY = 1.6f;
    [SerializeField] private float crouchCameraY = 0.8f;
    [SerializeField] private float lerpSpeed = 14f;

    public bool IsSliding => _isSliding;
    public bool IsActive => _isSliding;

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

        _slideVelocity = Vector3.MoveTowards(_slideVelocity, Vector3.zero,
            slideFriction * Time.fixedDeltaTime);

        Vector3 currentHorizontal = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        return _slideVelocity - currentHorizontal;
    }

    private IEnumerator SlideRoutine()
    {
        _isSliding = true;
        _slideVelocity = transform.forward * slideForce;
        _motor.Controller.height = crouchHeight;

        yield return new WaitForSeconds(slideDuration);

        EndSlide();
    }

    private void EndSlide()
    {
        _isSliding = false;
        _slideVelocity = Vector3.zero;
        _motor.Controller.height = normalHeight;
    }
}