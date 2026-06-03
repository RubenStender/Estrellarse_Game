using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Owns the CharacterController and applies final velocity.
/// Collects all IMovementModifier components on this GameObject and sums their contributions.
/// Add/remove movement modules freely — this script never needs to change.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedGravity = -2f; // Small downward force to keep grounded

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask = ~0;

    // Public read access for modules
    public Vector3 Velocity => _velocity;
    public bool IsGrounded { get; private set; }
    public CharacterController Controller => _controller;

    private CharacterController _controller;
    private Vector3 _velocity;
    private List<IMovementModifier> _modifiers = new();

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()  => RefreshModifiers();
    private void OnDisable() => RefreshModifiers();

    /// <summary>Call this if you add/remove a movement module at runtime.</summary>
    public void RefreshModifiers()
    {
        _modifiers.Clear();
        GetComponents(_modifiers);
    }

    private void FixedUpdate()
    {
        IsGrounded = CheckGrounded();

        // Let each module contribute
        Vector3 delta = Vector3.zero;
        foreach (var mod in _modifiers)
            delta += mod.GetVelocityDelta(_velocity, IsGrounded);

        _velocity += delta;

        // Apply gravity separately so modules don't fight it
        float g = IsGrounded ? groundedGravity : gravity;
        _velocity.y += g * Time.fixedDeltaTime;

        // Ground snap — don't let y accumulate while grounded
        if (IsGrounded && _velocity.y < 0f)
            _velocity.y = groundedGravity;

        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Modules can call this to override vertical velocity (e.g. jump).
    /// </summary>
    public void SetVerticalVelocity(float y) => _velocity.y = y;

    /// <summary>
    /// Modules can call this to inject a one-frame impulse (e.g. dash, knockback).
    /// </summary>
    public void AddImpulse(Vector3 impulse) => _velocity += impulse;

    /// <summary>
    /// Zero out horizontal velocity (used by slide end, wall-run end, etc).
    /// </summary>
    public void ZeroHorizontalVelocity()
    {
        _velocity.x = 0f;
        _velocity.z = 0f;
    }

    private bool CheckGrounded()
    {
        // Cast from the bottom of the controller downward
        Vector3 bottom = transform.position
                       + _controller.center
                       + Vector3.down * (_controller.height / 2f - _controller.radius);

        return Physics.SphereCast(bottom, _controller.radius * 0.9f,
            Vector3.down, out _, groundCheckDistance + 0.1f, groundMask);
    }
}
