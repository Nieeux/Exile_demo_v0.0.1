using UnityEngine;

public class PickupSpeed : Pickup
{
    //Lượng máu hồi khi nhặt
    public float amount = 1;

    protected override void OnPicked(Player other)
    {
        PlayerStats Speed = other.GetComponent<PlayerStats>();
        if (Speed)
        {
            Speed.Speed(amount);
            Destroy(gameObject);
        }
    }
}