using UnityEngine;

public class PickupHealth : Pickup
{
    //Lượng máu hồi khi nhặt
    public float amount = 1;

    protected override void OnPicked(Player other)
    {
        PlayerStats health = other.GetComponent<PlayerStats>();
        if (health)
        {
            health.Heal(amount);
            Destroy(gameObject);
        }
    }
}