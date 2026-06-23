using UnityEngine;

public class ShotgunAmmo : MonoBehaviour, IUsableItem
{
    public int ammoAmount = 1;

    public void AddToShotgun()
    {
        ShotgunWeapon shotgun = FindFirstObjectByType<ShotgunWeapon>();

        if (shotgun != null)
        {
            shotgun.AddAmmo(ammoAmount);
        }
    }

    public void UseItem()
    {
    }
}