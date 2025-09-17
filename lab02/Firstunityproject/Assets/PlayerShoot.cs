using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private WeaponFire weaponFire;

    void Awake()
    {
        Debug.Log("start weapon fire");

        weaponFire = GetComponent<WeaponFire>();
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            weaponFire.Fire();
            Debug.Log("Projectile fired!");
        }
    }
}
