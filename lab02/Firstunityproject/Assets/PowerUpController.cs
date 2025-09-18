using System.Collections.Generic;        // for List
using UnityEngine;                       // Unity types

// Kinds of simple power‑ups we support.
public enum PowerUpType { SpeedMultiplier, ShieldPoints, InstantHeal }

// Tracks timed effects and exposes current move speed.
public class PowerUpController : MonoBehaviour {
    [Header("Base stats")]
    [SerializeField] private float baseMoveSpeed = 5f; // your normal speed

    // Read‑only current values other scripts can consume.
    public float CurrentMoveSpeed { get; private set; } = 5f;
    public int CurrentShield { get; private set; } = 0;

    // Internal list of active timed effects.
    private readonly List<ActiveEffect> effects = new List<ActiveEffect>();

    void Awake() { CurrentMoveSpeed = baseMoveSpeed; }

    void Update() {
        // Tick down timers and remove expired effects.
        for (int i = effects.Count - 1; i >= 0; i--) {
            var e = effects[i]; e.timeLeft -= Time.deltaTime; effects[i] = e;
            if (effects[i].timeLeft <= 0f) effects.RemoveAt(i);
        }
        // Recalculate derived values each frame (simple and safe).
        float speedMul = 1f; int shield = 0;
        foreach (var e in effects) {
            if (e.type == PowerUpType.SpeedMultiplier) speedMul *= e.value;
            else if (e.type == PowerUpType.ShieldPoints) shield += Mathf.RoundToInt(e.value);
        }
        CurrentMoveSpeed = baseMoveSpeed * speedMul;
        CurrentShield = shield;
    }

    public void ApplyPowerUp(PowerUpType type, float value, float durationSeconds) {
        // Instant effects act immediately and do not get stored.
        if (type == PowerUpType.InstantHeal) {
            var h = GetComponent<Health>();
            if (h) h.ApplyHeal(value);
            return;
        }
        // Timed effects are stored and recalculated in Update.
        effects.Add(new ActiveEffect { type = type, value = value, timeLeft = Mathf.Max(0f, durationSeconds) });
    }

    // Small struct to hold an active effect.
    private struct ActiveEffect { public PowerUpType type; public float value; public float timeLeft; }
}
