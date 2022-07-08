using UnityEngine;

public class PickupUpdateDamageWeapon : Pickup
{
    public float amount = 25;

    protected override void OnPicked(PlayerMovement other)
    {
        WeaponStats increate = other.GetComponent<WeaponStats>();
        if (increate)
        {
            increate.UpdateWeapon(amount);
            Destroy(gameObject);
        }
    }
}
