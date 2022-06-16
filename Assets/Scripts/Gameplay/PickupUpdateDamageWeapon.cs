using UnityEngine;

public class PickupUpdateDamageWeapon : Pickup
{
    public float amount = 25;

    protected override void OnPicked(Player other)
    {
        WeaponStats increate = other.GetComponent<WeaponStats>();
        if (increate)
        {
            increate.UpdateWeapon(amount);
            Destroy(gameObject);
        }
    }
}
