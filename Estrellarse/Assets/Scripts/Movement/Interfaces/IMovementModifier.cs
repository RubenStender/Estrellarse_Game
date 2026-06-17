using UnityEngine;

/// <summary>
/// Any script that wants to influence character movement implements this.
/// CharacterMotor collects all modifiers and sums their velocity contributions.
/// </summary>
public interface IMovementModifier
{
    /// <summary>
    /// Called every FixedUpdate by CharacterMotor.
    /// Return the velocity delta this module wants to add this frame.
    /// </summary>
    Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded);

    /// <summary>
    /// Whether this modifier is currently active (used for debug/priority).
    /// </summary>
    bool IsActive { get; }
}
