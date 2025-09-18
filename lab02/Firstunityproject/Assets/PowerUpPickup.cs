using System.Collections.Generic;        // for List
using UnityEngine; 

// Place on a pickup with a 2D trigger collider.
[RequireComponent(typeof(Collider2D))]
public class PowerUpPickup : MonoBehaviour {
    [Header("Effect settings")]
    public PowerUpType type = PowerUpType.SpeedMultiplier; // what to apply
    public float value = 3f;             // multiplier or amount
    public float duration = 5f;            // seconds (ignored for InstantHeal)

    [Header("Pickup")]
    public string requiredTag = "Player";  // who can pick up
    public bool destroyOnPickup = true;    // remove after use

    void Reset() { var c = GetComponent<Collider2D>(); if (c) c.isTrigger = true; }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag(requiredTag)) return;  // only intended target
        if (other.TryGetComponent<PowerUpController>(out var ctrl)) {
            ctrl.ApplyPowerUp(type, value, duration); // apply effect
            Debug.Log($"PowerUp applied: {type}, value: {value}");
            if (destroyOnPickup) Destroy(gameObject); // clean up
        }
    }
}