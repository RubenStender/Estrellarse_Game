using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedGravity = -2f;
    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask = ~0;

    public Vector3 Velocity => _velocity;
    public bool IsGrounded { get; private set; }
    public CharacterController Controller => _controller;
    public float GravityScale { get; set; } = 1f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private List<IMovementModifier> _modifiers = new();

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable() => RefreshModifiers();
    private void OnDisable() => RefreshModifiers();

    public void RefreshModifiers()
    {
        _modifiers.Clear();
        GetComponents(_modifiers);
    }

    private void FixedUpdate()
    {
        IsGrounded = CheckGrounded();

        Vector3 delta = Vector3.zero;
        foreach (var mod in _modifiers)
            delta += mod.GetVelocityDelta(_velocity, IsGrounded);
        _velocity += delta;

        float g = IsGrounded ? groundedGravity : gravity * GravityScale;
        _velocity.y += g * Time.fixedDeltaTime;

        if (IsGrounded && _velocity.y < 0f)
            _velocity.y = groundedGravity;

        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    public void SetVerticalVelocity(float y) => _velocity.y = y;
    public void AddImpulse(Vector3 impulse) => _velocity += impulse;
    public void ZeroHorizontalVelocity()
    {
        _velocity.x = 0f;
        _velocity.z = 0f;
    }

    private bool CheckGrounded()
    {
        Vector3 bottom = transform.position
                       + _controller.center
                       + Vector3.down * (_controller.height / 2f - _controller.radius);
        return Physics.SphereCast(bottom, _controller.radius * 0.9f,
            Vector3.down, out _, groundCheckDistance + 0.1f, groundMask);
    }
}