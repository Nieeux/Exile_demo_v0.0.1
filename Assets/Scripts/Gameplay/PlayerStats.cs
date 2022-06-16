using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth = 100;
    public float maxSpeed = 25f;
    public float walkingSpeed = 5f;

    public Player player;
    public WeaponController Weapon;

    public static PlayerStats Instance;


    private void Awake()
    {
        PlayerStats.Instance = this;
    }

    //Hồi phục khi nhặt máu
    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);

    }

    public void Speed(float amount)
    {
        walkingSpeed += amount;
        walkingSpeed = Mathf.Min(walkingSpeed, maxSpeed);
    }

    public void OnHeal(float amount)
    {

    }

    public int Hp()
    {
        return (int)(this.CurrentHealth);
    }

    public int MaxHp()
    {
        return (int)(this.MaxHealth);
    }

    // Nhận sát thương
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            //Player is dead
            player.canMove = false;
            Weapon.canFire = false;
            CurrentHealth = 0;
        }
    }
}