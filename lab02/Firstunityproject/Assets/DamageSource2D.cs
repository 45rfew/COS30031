using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageSource2D : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;
    [SerializeField] private GameObject owner;          // who caused it (optional)
    [SerializeField] private bool destroyOnHit = true;  // common for projectiles

    void Reset() {                                     // helpful defaults
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;                           // use triggers for hits
    }

    public void Configure(float dmg, DamageType type, GameObject src) {
        damage = Mathf.Max(0f, dmg);
        damageType = type;
        owner = src;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == owner) return;

        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.ApplyDamage(new DamageInfo(damage, damageType, owner ? owner : gameObject));
            if (destroyOnHit) Destroy(gameObject);
        }
    }
}