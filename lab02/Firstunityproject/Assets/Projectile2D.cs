using UnityEngine;                       // Unity types

// Basic 2D projectile. Either uses Rigidbody2D velocity or Translate.
[RequireComponent(typeof(Collider2D))]
public class Projectile2D : MonoBehaviour {
    [Header("Motion")]
    [SerializeField] private float speed = 12f;      // units per second
    [SerializeField] private Vector2 direction = Vector2.right; // local forward
    [SerializeField] private bool useRigidbody = true; // prefer physics when true
    [SerializeField] private bool faceVelocity = true; // rotate to face motion

    [Header("Lifetime")]
    [SerializeField] private float maxLife = 3f;     // seconds before despawn

    private Rigidbody2D rb;           // cached body
    private float deathTime;          // when to despawn

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    [System.Obsolete]
    void OnEnable() {
        deathTime = Time.time + maxLife;             // schedule despawn
        if (useRigidbody && rb) {
            rb.velocity = direction.normalized * speed; // set velocity
        }
    }

    [System.Obsolete]
    void Update() {
        if (!useRigidbody || !rb) {
            // Move by Translate if no Rigidbody motion.
            transform.Translate((Vector3)(direction.normalized * speed * Time.deltaTime), Space.World);
        }
        if (faceVelocity) {
            // Point the sprite along the velocity/direction.
            Vector2 vel = useRigidbody && rb ? rb.velocity : direction * speed;
            if (vel.sqrMagnitude > 0.0001f) {
                float ang = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, ang);
            }
        }
        if (Time.time >= deathTime) Destroy(gameObject); // time‑out
    }

    // Allow code to set direction at fire time.
    [System.Obsolete]
    public void Fire(Vector2 dir, float spd) {
        direction = dir; speed = spd;
        if (useRigidbody && rb) rb.velocity = dir.normalized * spd;
    }
}