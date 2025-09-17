using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour {
    public float damage = 10f;
    public DamageType damageType = DamageType.Physical; // choose in Inspector

    void OnValidate() {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = false;
    }

    void OnCollisionEnter2D(Collision2D col) {
        var target = col.collider.GetComponentInParent<IDamageable>();
        if (target == null) return;

        // Create the packet
        var info = new DamageInfo(damage, damageType, gameObject);

        target.ApplyDamage(info);
    }
}
