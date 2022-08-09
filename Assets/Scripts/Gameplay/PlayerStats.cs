using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Heath")]
    public float MaxHealth = 100;
    public float CurrentHealth { get; set; }
    public float armor { get; set; }
    public int speed { get; set; }

    [Header("Hunger")]
    private float hungerDrainRate = 0.15f;
    public float hunger { get; set; }
    public float maxHunger { get; set; }

    [Header("Stamina")]
    private bool canRun = true;
    public float stamina { get; set; }
    public float maxStamina { get; set; }
    private float staminaRegenRate = 15f;
    private float staminaDrainRate = 12f;
    private float staminaDrainMultiplier = 5f;
    private float healingDrainMultiplier = 2f;
    private Coroutine regeneratingHealth;

    [Header("Healing")]
    private bool healing;
    private float healingRate = 5f;

    [Header("Damage")]
    bool IsDead;
    public float damage { get; set; }
    public bool Invincible { get; set; }
    public float DamageMultiplier = 1f;

    private PlayerMovement player;
    public UnityAction<float> OnDamaged;
    public UnityAction<float> OnHealed;
    public UnityAction OnDie;


    private void Awake()
    {
        PlayerStats.Instance = this;
        this.player = base.GetComponent<PlayerMovement>();
        this.damage = 50f;
        this.stamina = 100f;
        this.hunger = 100f;
        this.maxStamina = this.stamina;
        this.maxHunger = this.hunger;
        this.CurrentHealth = this.MaxHealth;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Stamina();
        Hunger();
        Healing();
    }
    private void Stamina()
    {
        if (!player.isRunning)
        {
            if (this.stamina < 100f && this.hunger > 0f)
            {
                float num = 1f;
                if (this.hunger <= 0f)
                {
                    num *= 0.3f;
                }
                this.stamina += this.staminaRegenRate * Time.deltaTime * num;
            }
        }
        else
        {
            this.stamina -= this.staminaDrainRate * Time.deltaTime / 2f;
        }

        if (this.stamina <= 0f)
        {
            this.stamina = 0;
            regeneratingHealth = StartCoroutine(CanRegeneratingHealth());
        }
        if (regeneratingHealth != null)
        {
            StopCoroutine(CanRegeneratingHealth());
            regeneratingHealth = null;
        }
    }
    private IEnumerator CanRegeneratingHealth()
    {
        canRun = false;
        yield return new WaitForSeconds(3);
        canRun = true;
        regeneratingHealth = null;

    }
    private void Hunger()
    {
        if (this.hunger <= 0f || this.CurrentHealth <= 0f)
        {
            return;
        }
        float num = 0.2f;
        if (this.healing)
        {
            num *= this.healingDrainMultiplier;
        }
        if (this.player.isRunning)
        {
            num *= this.staminaDrainMultiplier;
        }
        this.hunger -= this.hungerDrainRate * Time.deltaTime * num;
        if (this.hunger < 0f)
        {
            this.hunger = 0f;
        }
    }
    private void Healing()
    {
        if (this.CurrentHealth <= 0f || this.CurrentHealth >= (float)this.MaxHealth || this.hunger <= 0f)
        {
            return;
        }
        float num = this.healingRate * Time.deltaTime * 0.1f;
        this.CurrentHealth += num;
    }

    public void Eat(ItemStats item)
    {

    }
    public void Armor(ItemStats item)
    {

    }
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

    public void Damage(float damage)
    {
        if (Invincible)
            return;

        CameraShake.Instance.Shake();
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

    public void InflictDamage(float damage, bool isExplosionDamage)
    {
        var totalDamage = damage;

        // skip the crit multiplier if it's from an explosion
        if (!isExplosionDamage)
        {
            totalDamage *= DamageMultiplier;
        }

        // apply the damages
        Damage(totalDamage);
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
    public bool CanRun()
    {
        return this.stamina > 0f && canRun == true;
    }
    public bool Weak()
    {
        return this.stamina == 100f;
    }
}
