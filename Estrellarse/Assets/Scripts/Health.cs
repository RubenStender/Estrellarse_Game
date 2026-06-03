using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple health component. Used by Shooter (hitscan) and Melee (overlap).
/// Works on any GameObject — player, enemy, destructible prop, whatever.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth    = 100f;
    [SerializeField] private bool  destroyOnDeath = false;

    [Header("Events")]
    public UnityEvent<float, float> OnHealthChanged; // (current, max)
    public UnityEvent<float, GameObject> OnDamaged;  // (amount, source)
    public UnityEvent<GameObject> OnDeath;           // (killer)

    public float CurrentHealth { get; private set; }
    public float MaxHealth     => maxHealth;
    public bool  IsDead        { get; private set; }
    public float HealthPercent => CurrentHealth / maxHealth;

    private void Awake() => CurrentHealth = maxHealth;

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnDamaged?.Invoke(amount, source);

        if (CurrentHealth <= 0f) Die(source);
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void SetMaxHealth(float newMax, bool healToFull = false)
    {
        maxHealth = newMax;
        if (healToFull) CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die(GameObject killer)
    {
        IsDead = true;
        OnDeath?.Invoke(killer);
        if (destroyOnDeath) Destroy(gameObject);
    }
}
