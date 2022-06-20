using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float MaxHealth = 100;

    bool IsDead;

    public UnityAction<float> OnDamaged;
    public UnityAction<float> OnHealed;
    public UnityAction OnDie;

    public float CurrentHealth { get; set; }
    public bool Invincible { get; set; }

    private void Awake()
    {

    }

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    //Hồi phục khi nhặt máu
    public void Heal(float healAmount)
    {
        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // call OnHeal action
        float trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f)
        {
            OnHealed?.Invoke(trueHealAmount);
        }
    }

    // Nhận sát thương
    public void TakeDamage(float damage)
    {
        if (Invincible)
            return;

        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f)
        {
            OnDamaged?.Invoke(trueDamageAmount);
        }
        HandleDeath();
    }
    public void Kill()
    {
        CurrentHealth = 0f;

        // call OnDamage action
        OnDamaged?.Invoke(MaxHealth);

        HandleDeath();
    }

    void HandleDeath()
    {
        if (IsDead)
            return;

        // call OnDie action
        if (CurrentHealth <= 0f)
        {
            IsDead = true;
            OnDie?.Invoke();
        }
    }
}