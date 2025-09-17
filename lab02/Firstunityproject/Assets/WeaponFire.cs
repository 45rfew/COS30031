using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzle;       // spawn point
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private DamageType damageType = DamageType.Physical;

    [System.Obsolete]
    public void Fire() {
        if (!projectilePrefab) return;
        
        var pos = muzzle ? muzzle.position : transform.position;
        var rot = muzzle ? muzzle.rotation : transform.rotation;

        var p = GameObject.Instantiate(projectilePrefab, pos, rot);
        var rb = p.GetComponent<Rigidbody2D>();
        if (rb) rb.velocity = transform.right * projectileSpeed;

        var src = p.GetComponent<DamageSource2D>();
        if (src) src.Configure(damage, damageType, gameObject);
        Debug.Log("Projectile spawned at: " + p.transform.position);

    }

}