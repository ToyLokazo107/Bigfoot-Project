using UnityEngine;

public class ShotgunAmmoPickup : InteractableObject
{
    public int ammoAmount = 1;

    public override void Interact()
    {
        ShotgunWeapon shotgun = FindFirstObjectByType<ShotgunWeapon>();

        if (shotgun != null)
        {
            shotgun.AddAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
