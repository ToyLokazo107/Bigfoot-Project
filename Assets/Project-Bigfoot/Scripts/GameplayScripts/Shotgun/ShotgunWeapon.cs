using UnityEngine;

public class ShotgunWeapon : MonoBehaviour, IUsableItem
{
    public Camera playerCamera;
    public Transform muzzlePoint;
    public GameObject bulletPrefab;

    public int currentAmmo = 3;
    public int ammoInMap = 10;
    public int maxAmmo = 13;

    public int damage = 10;
    public float shootRange = 100f;

    public void UseItem()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo");
            CheckRageCondition();
            return;
        }

        currentAmmo--;

        Vector3 targetPoint = playerCamera.transform.position + playerCamera.transform.forward * shootRange;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootRange))
        {
            targetPoint = hit.point;
        }

        Vector3 direction = (targetPoint - muzzlePoint.position).normalized;

        Vector3 shootDirection = playerCamera.transform.forward;

        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            Quaternion.LookRotation(shootDirection)
        );

        ShotgunBullet bulletScript = bullet.GetComponent<ShotgunBullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDamage(damage);
        }

        Debug.Log("Ammo: " + currentAmmo + " | Ammo in map: " + ammoInMap);

        CheckRageCondition();
    }

    public void AddAmmo(int amount)
    {
        if (currentAmmo >= maxAmmo)
            return;

        currentAmmo += amount;
        ammoInMap -= amount;

        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        ammoInMap = Mathf.Clamp(ammoInMap, 0, 10);

        Debug.Log("Ammo added. Ammo: " + currentAmmo + " | Ammo in map: " + ammoInMap);
    }

    private void CheckRageCondition()
    {
        if (currentAmmo <= 0 && ammoInMap <= 0)
        {
            Debug.Log("Bigfoot Rage Mode");
        }
    }
}