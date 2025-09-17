using UnityEngine;

public enum DamageType { Physical, Fire, Poison, True }
public struct DamageInfo
{
    public float Amount;                 // how much damage
    public DamageType Type;              // what kind
    public GameObject Source;            // who caused it (optional)
    public DamageInfo(float amount, DamageType type, GameObject source = null)
    {
        Amount = Mathf.Max(0f, amount);  // clamp to non‑negative
        Type = type;                     // set type
        Source = source;                 // store source
    }
}