using UnityEngine;

public interface IMovementModifier
{
    Vector3 GetVelocityDelta(Vector3 currentVelocity, bool isGrounded);
    bool IsActive { get; }
}
