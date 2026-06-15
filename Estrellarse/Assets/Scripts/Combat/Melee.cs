using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Melee attack. Uses an overlap sphere at a hitbox point.
/// Timing windows (startup → active → recovery) prevent spam and feel snappy.
/// Works on player or enemy — just call TryMelee().
/// </summary>
public class Melee : MonoBehaviour
{
    [Header("Melee Settings")]
    [SerializeField] private float damage       = 50f;
    [SerializeField] private float hitRange     = 1.5f;
    [SerializeField] private float knockback    = 8f;
    [SerializeField] private LayerMask hitMask  = ~0;

    [Header("Timing (seconds)")]
    [SerializeField] private float startupTime  = 0.08f;  // Before hit registers
    [SerializeField] private float activeTime   = 0.1f;   // Hit window
    [SerializeField] private float recoveryTime = 0.25f;  // Cooldown after swing

    [Header("Hitbox")]
    [SerializeField] private Transform hitboxOrigin;       // If null, uses transform + forward offset

    [Header("Events")]
    public UnityEvent OnMeleeStart;
    public UnityEvent<GameObject> OnMeleeHit;
    public UnityEvent OnMeleeEnd;

    public bool IsAttacking { get; private set; }

    // Called by InputBridge or AI
    public void TryMelee()
    {
        if (IsAttacking) return;
        StartCoroutine(MeleeRoutine());
    }

    private void Awake()
    {
        if (hitboxOrigin == null) hitboxOrigin = transform;
    }

    private IEnumerator MeleeRoutine()
    {
        IsAttacking = true;
        OnMeleeStart?.Invoke();

        // Startup — wind up
        yield return new WaitForSeconds(startupTime);

        // Active — check hits
        Vector3 origin = hitboxOrigin.position + transform.forward * (hitRange * 0.5f);
        Collider[] hits = Physics.OverlapSphere(origin, hitRange * 0.5f, hitMask);

        HashSet<GameObject> alreadyHit = new();
        foreach (var col in hits)
        {
            if (col.gameObject == gameObject) continue;
            if (alreadyHit.Contains(col.gameObject)) continue;
            alreadyHit.Add(col.gameObject);

            if (col.TryGetComponent<Health>(out var health))
                health.TakeDamage(damage, gameObject);

            // Apply knockback
            if (col.TryGetComponent<CharacterMotor>(out var motor))
            {
                Vector3 dir = (col.transform.position - transform.position).normalized;
                motor.AddImpulse(dir * knockback);
            }

            OnMeleeHit?.Invoke(col.gameObject);
        }

        yield return new WaitForSeconds(activeTime);

        // Recovery — can't attack again until this ends
        yield return new WaitForSeconds(recoveryTime);

        IsAttacking = false;
        OnMeleeEnd?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        if (hitboxOrigin == null) return;
        Gizmos.color = Color.red;
        Vector3 origin = hitboxOrigin.position + transform.forward * (hitRange * 0.5f);
        Gizmos.DrawWireSphere(origin, hitRange * 0.5f);
    }
}
