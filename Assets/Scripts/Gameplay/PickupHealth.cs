using UnityEngine;

public class PickupHealth : Pickup
{
    //Lượng máu hồi khi nhặt
    public float amount = 1;

    protected override void OnPicked(PlayerMovement other)
    {
        HitAble health = other.GetComponent<HitAble>();
        if (health)
        {
            health.Heal(amount);
            Destroy(gameObject);
        }
    }
}