using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Dash : MonoBehaviour, IMovementModifier
{
    [Header("Dash Settings")]
    [SerializeField] private float dashForce     = 35f;
    [SerializeField] private float dashDuration  = 0.18f;
    [SerializeField] private float cooldown      = 1.2f;
    [SerializeField] private int   dashCharges   = 1;        
    [SerializeField] private bool  canDashInAir  = true;

    [Header("Feel")]
    [SerializeField] private float dashGravityScale = 0f;    

    public bool IsActive     => _isDashing;
    public bool IsOnCooldown => _chargesLeft <= 0;
    public float CooldownProgress => 1f - (_cooldownTimer / cooldown); 

 
    public void TryDash(Vector3 worldDirection)
    {
        if (_isDashing)            return;
        if (_chargesLeft <= 0)     return;
        if (!canDashInAir && !_motor.IsGrounded) return;

        StartCoroutine(DashRoutine(worldDirection));
    }

    private CharacterMotor _motor;
    private bool  _isDashing;
    private int   _chargesLeft;
    private float _cooldownTimer;
    private Vector3 _dashVelocity;

    private void Awake()
    {
        _motor = GetComponent<CharacterMotor>();
        _chargesLeft = dashCharges;
    }

    private void Update()
    {
        if (_chargesLeft < dashCharges)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                _chargesLeft++;
                _cooldownTimer = cooldown;
            }
        }
    }

    public Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded)
    {
        if (!_isDashing) return Vector3.zero;

        // Override current horizontal velocity to dash velocity
        Vector3 current = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        return _dashVelocity - current;
    }

    private IEnumerator DashRoutine(Vector3 direction)
    {
        _isDashing   = true;
        _chargesLeft--;
        _cooldownTimer = cooldown;

        _dashVelocity = direction.normalized * dashForce;

        // Optionally freeze Y during dash
        if (dashGravityScale == 0f)
            _motor.SetVerticalVelocity(0f);

        yield return new WaitForSeconds(dashDuration);

        _dashVelocity = Vector3.zero;
        _isDashing    = false;
    }
}
