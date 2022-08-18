using UnityEngine;
using UnityEngine.Events;

public class HitAble : MonoBehaviour
{
    public float MaxHealth = 100;
    public float DamageMultiplier = 1f;
    public GameObject numberFx;
    public ItemStats currentArmor;
    public float damageFinal;
    public int damageFinalEffect;

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

    public void Damage(float damage, Bullet bulletType, bool Crit, Vector3 pos)
    {
        if (Invincible)
            return;

        float healthBefore = CurrentHealth;

        if (currentArmor != null)
        {
            DamageCalculations.ArmorResult damageMultiplier = DamageCalculations.Instance.GetDamageArmor(currentArmor, bulletType, Crit, damage);
            damageFinal = damageMultiplier.damageResult;
            damageFinalEffect = (int)damageMultiplier.hitType;
        }
        else
        {
            damageFinal = damage;
        }

        CurrentHealth -= (int)damageFinal;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        Vector3 normalized = (PlayerMovement.Instance.playerCamera.position + Vector3.up * 1.5f - pos).normalized;


        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;

        if (Vector3.Distance(PlayerMovement.Instance.playerCamera.position, base.transform.position) < 100f)
        {
            Object.Instantiate<GameObject>(this.numberFx, pos, Quaternion.identity).GetComponent<HitEffect>().SetTextAndDir((float)trueDamageAmount, normalized, (HitEffect.HitType)damageFinalEffect);
        }

        if (trueDamageAmount > 0f)
        {
            OnDamaged?.Invoke(trueDamageAmount);
        }
        HandleDeath();
    }

    /*
    public void InflictDamage(float damage, bool isExplosionDamage, int hitEffect, Vector3 pos)
    {
        var totalDamage = damage;

        // skip the crit multiplier if it's from an explosion
        if (!isExplosionDamage)
        {
            totalDamage *= DamageMultiplier;
        }

        // apply the damages
        Damage(totalDamage, hitEffect, pos);
    }
    */

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