using UnityEngine;

public class PickupHealth : Pickup
{
    //Lượng máu hồi khi nhặt
    public float amount = 1;

    protected override void OnPicked(Player other)
    {
        Health health = other.GetComponent<Health>();
        if (health)
        {
            health.Heal(amount);
            Destroy(gameObject);
        }
    }
}