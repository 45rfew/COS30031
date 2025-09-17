using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable {
    [Header("Numbers")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool startAtMax = true;
    [SerializeField] private bool clampOverheal = true;

    [Header("Invulnerability")]
    [SerializeField] private float invulnAfterHit = 0.0f;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action<DamageInfo> OnDamaged;
    public event Action<float> OnHealed;
    public event Action OnDeath;

    private bool dead;
    private float lastHitTime = -999f;

    void Awake() {
        CurrentHealth = startAtMax ? maxHealth : Mathf.Clamp(CurrentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void ApplyDamage(DamageInfo info) {
        if (dead) return;
        if (Time.time - lastHitTime < invulnAfterHit) return;

        float newHealth = Mathf.Max(0f, CurrentHealth - info.Amount);

        Debug.Log($"{gameObject.name} took {info.Amount} {info.Type} damage from {info.Source?.name ?? "Unknown"} (was {CurrentHealth}, now {newHealth})");

        if (Mathf.Approximately(newHealth, CurrentHealth)) return;

        CurrentHealth = newHealth;
        lastHitTime = Time.time;

        OnDamaged?.Invoke(info);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (!dead && CurrentHealth <= 0f) {
            dead = true;
            Debug.Log($"{gameObject.name} has died.");
            OnDeath?.Invoke();
        }
    }

    public void ApplyHeal(float amount) {
        if (dead) return;

        float target = CurrentHealth + Mathf.Max(0f, amount);
        CurrentHealth = clampOverheal ? Mathf.Min(target, maxHealth) : target;

        Debug.Log($"{gameObject.name} healed {amount}. Current health: {CurrentHealth}/{maxHealth}");

        OnHealed?.Invoke(amount);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}
