using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool destroyOnDeath = false;

    [Header("Unity Events (Inspector)")]
    public UnityEvent<float, float> OnHealthChangedEvent;
    public UnityEvent<float, GameObject> OnDamagedEvent;
    public UnityEvent<GameObject> OnDeathEvent;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, GameObject> OnDamaged;
    public event Action<GameObject> OnDeath;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }
    public float HealthPercent => CurrentHealth / maxHealth;

    private void Awake() => CurrentHealth = maxHealth;

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnHealthChangedEvent?.Invoke(CurrentHealth, maxHealth);

        OnDamaged?.Invoke(amount, source);
        OnDamagedEvent?.Invoke(amount, source);

        if (CurrentHealth <= 0f)
            Die(source);
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnHealthChangedEvent?.Invoke(CurrentHealth, maxHealth);
    }

    public void SetMaxHealth(float newMax, bool healToFull = false)
    {
        maxHealth = newMax;
        if (healToFull) CurrentHealth = maxHealth;

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnHealthChangedEvent?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die(GameObject killer)
    {
        IsDead = true;

        OnDeath?.Invoke(killer);
        OnDeathEvent?.Invoke(killer);

        if (destroyOnDeath)
            Destroy(gameObject);
    }
}