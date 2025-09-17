using System.ComponentModel.Design.Serialization;
using UnityEngine;

public interface IDamageable {
    float CurrentHealth { get; }         // read current health
    float MaxHealth { get; }             // read max health
    void ApplyDamage(DamageInfo info);   // take a DamageInfo packet
    void ApplyHeal(float amount);        // heal by amount
}