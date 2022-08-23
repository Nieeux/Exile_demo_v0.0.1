using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [SerializeField]
    private float MaxHealth = 100;
    public float CurrentHealth { get; private set; }
    public float armor { get; private set; }

    [Header("Hunger")]
    [SerializeField]
    private float hungerDrainRate = 1f;
    public float hunger { get; private set; }
    public float maxHunger { get; private set; }


    [Header("Sleepy")]
    [SerializeField]
    private float SleepyDrainRate = 1f;
    public float sleepy { get; private set; }
    public float maxSleepy { get; private set; }
    private bool Sleeping;
    private bool canSleep;

    [Header("Stamina")]
    private bool canRun = true;
    public float stamina { get; private set; }
    public float maxStamina { get; private set; }
    private float staminaRegenRate = 12f;
    private float staminaDrainRate = 12f;
    private float staminaDrainMultiplier = 5f;
    private float healingDrainMultiplier = 2f;
    private Coroutine regeneratingStamina;

    [Header("Healing")]
    private bool healing;
    private float healingRate = 5f;
    //Buff
    private float healingRateBuff = 0;

    [Header("Damage")]
    public float damageFinal;
    private bool IsDead;
    public float damage { get; private set; }
    public bool Invincible { get; private set; }
    public float DamageMultiplier = 1f;

    [Header("Damage")]
    public float currentWeight;
    private float maxWeight;
    //Equip
    private float IncreaseMaxWeight;
    //Buff
    private float BioTechBuff;

    [Header("Speed")]
    //Equip
    private float IncreaseSpeedEquip;
    //Buff
    private float IncreaseSpeedBuff;

    private PlayerMovement player;
    Inventory inventory;
    WeaponInventory weaponInventory;

    public UnityAction<float> OnDamaged;
    public UnityAction<float> OnHealed;
    public UnityAction OnSleep;
    public UnityAction OnWakeUp;
    public UnityAction OnExhausted;
    public UnityAction OnDie;



    private void Awake()
    {
        PlayerStats.Instance = this;
        this.player = base.GetComponent<PlayerMovement>();

        weaponInventory = GetComponent<WeaponInventory>();
        weaponInventory.bioTech += GetWeightBuff;
        weaponInventory.speed += GetSpeedUpBuff;
        weaponInventory.heal += GethealingBuff;

        inventory = GetComponent<Inventory>();
        inventory.weight += MaxWeight;
        inventory.speed += GetSpeedUp;
        inventory.maxHealth += GetMaxHealthUp;

        this.damage = 50f;
        this.stamina = 100f;
        this.hunger = 100f;
        this.sleepy = 100f;
        this.maxWeight = 20f;
        this.maxStamina = this.stamina;
        this.maxHunger = this.hunger;
        this.maxSleepy = this.sleepy;
        this.CurrentHealth = this.MaxHealth;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsDead)
        {
            return;
        }
        Stamina();
        Hunger();
        Healing();
        Hungry();
        Sleepy();
    }

    #region Stats
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
            regeneratingStamina = StartCoroutine(CanRegeneratingStamina());
        }
        if (regeneratingStamina != null)
        {
            StopCoroutine(CanRegeneratingStamina());
            regeneratingStamina = null;
        }
    }
    private IEnumerator CanRegeneratingStamina()
    {
        canRun = false;
        yield return new WaitForSeconds(3);
        canRun = true;
        regeneratingStamina = null;

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
        float num = (this.healingRate + healingRateBuff) * Time.deltaTime * 0.1f;
        this.CurrentHealth += num;
    }
    private void Hungry()
    {
        if (this.hunger <= 0f)
        {
            float num = this.healingRate * Time.deltaTime * 1f;
            this.CurrentHealth -= num;

            if (this.CurrentHealth <= 0)
                this.CurrentHealth = 0;
            HandleDeath();
        }

    }
    private void Sleepy()
    {

        if (this.sleepy >= 0f || this.CurrentHealth >= 0f)
        {

            if (this.sleepy == 0f || this.CurrentHealth == 0f || Sleeping == true)
            {
                return;
            }
            float num = 0.2f;

            this.sleepy -= this.SleepyDrainRate * Time.deltaTime * num;

            if (this.sleepy <= 25f)
            {
                canSleep = true;
            }
            else
            {
                canSleep = false;
            }

        }
        if (this.sleepy < 0f)
        {
            Sleep();
            this.sleepy = 0f;

        }
    }
    #endregion

    #region Eat
    public bool Coffee(ItemStats item)
    {
        if (sleepy >= maxSleepy)
        {
            return false;
        }

        sleepy += item.sleep;
        sleepy = Mathf.Clamp(sleepy, 0f, maxSleepy);
        return true;
    }

    public bool Eat(float Hunger)
    {
        if (hunger >= maxHunger)
        {
            return false;
        }

        this.hunger += Hunger;
        this.hunger = Mathf.Clamp(hunger, 0f, maxHunger);
        return true;
    }

    public bool Heal(ItemStats item)
    {
        if (CurrentHealth >= MaxHealth || IsDead)
        {
            return false;
        }
        float healthBefore = CurrentHealth;
        CurrentHealth += item.heal;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // call OnHeal action
        float trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f)
        {
            OnHealed?.Invoke(trueHealAmount);
        }
        return true;
    }
    #endregion

    #region SleepSystem
    private void CheckPlatformSleep()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("SleepOnGround");
                bool dead = Random.Range(0f, 1f) < 0.8f;
                if (dead)
                {
                    float random = Random.Range(5, 10);
                    base.Invoke("Dead", random);
                }
                else
                {
                    base.Invoke("WakeUp", 5f);
                }
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Hide"))
            {
                Debug.Log("SleepOnHide");
                bool dead = Random.Range(0f, 1f) < 0.5f;
                if (dead)
                {
                    float random = Random.Range(5, 10);
                    base.Invoke("Dead", random);
                }
                else
                {
                    base.Invoke("WakeUp", 5f);
                }
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Base"))
            {
                Debug.Log("SleepOnBase");
                base.Invoke("WakeUp", 5f);
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Structure"))
            {
                Debug.Log("SleepOnStructure");
                base.Invoke("WakeUp", 5f);
            }
        }
    }

    public void Sleep()
    {
        Sleeping = true;
        OnSleep?.Invoke();
        CheckPlatformSleep();
    }

    private void WakeUp()
    {
        OnWakeUp?.Invoke();
        hunger /= 2f;
        sleepy = maxSleepy;
        Sleeping = false;
        DayNightCycle.Instance.NewDay();
    }
    #endregion

    #region GetEquip
    private void GetMaxHealthUp(float maxHealth)
    {
        this.MaxHealth *= (1 + maxHealth);
        if(maxHealth == 0)
        {
            MaxHealth = 100;
        }
        if(CurrentHealth > MaxHealth)
        CurrentHealth = MaxHealth;

    }
    private void MaxWeight(float maxWeight)
    {
        float n = maxWeight;
        IncreaseMaxWeight = n;
    }

    private void GetSpeedUp(float Speed)
    {
        float n = Speed;
        IncreaseSpeedEquip = n;
    }

    public float GetMaxWeight()
    {
        return maxWeight + IncreaseMaxWeight;
    }

    public float Weight()
    {
        float n = inventory.GetWeight() + (weaponInventory.GetWeight() / (1 + BioTechBuff));
        return n;
    }

    public float CurrentSpeed()
    {
        float n = 3 + IncreaseSpeedEquip + IncreaseSpeedBuff;
        if (Weight() >= maxWeight + IncreaseMaxWeight)
        {
            return n /= 2;
        }
        return n;
    }
    #endregion

    #region GetBuff
    private void GethealingBuff(float critcal, bool recRefresh)
    {
        if (recRefresh == true)
        {
            this.healingRateBuff = critcal;
        }
        float n = critcal;

        this.healingRateBuff += n;
    }
    private void GetSpeedUpBuff(float SeepBuff, bool recRefresh)
    {
        if (recRefresh == true)
        {
            this.IncreaseSpeedBuff = SeepBuff;
        }
        float n = SeepBuff;
        this.IncreaseSpeedBuff += n;
    }
    private void GetWeightBuff(float WeightBuff, bool recRefresh)
    {
        if (recRefresh == true)
        {
            this.BioTechBuff = WeightBuff;
        }
        float n = WeightBuff;
        this.BioTechBuff += n;
    }
    #endregion

    #region GetData
    public float GetSleepyRatio()
    {
        return sleepy / maxSleepy;
    }
    public float GetHungerRatio()
    {
        return hunger / maxHunger;
    }
    public float GetHealthRatio()
    {
        return CurrentHealth / MaxHealth;
    }
    public float GetMaxHealth()
    {
        return MaxHealth;
    }
    public bool GetCanSleep()
    {
        if (canSleep == true)
        {
            return true;
        }
        return false;
    }
    #endregion

    public void Damage(float damage, Bullet bulletType)
    {
        if (Invincible)
            return;

        CameraShake.Instance.Shake();

        float healthBefore = CurrentHealth;
        bool Crit = false;

        if (inventory.currentArmor != null)
        {
            DamageCalculations.ArmorResult damageMultiplier = DamageCalculations.Instance.GetDamageArmor(inventory.currentArmor,bulletType, Crit, damage);
            damageFinal = damageMultiplier.damageResult;
        }
        else
        {
            damageFinal = damage;
        }

        CurrentHealth -= damageFinal;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);



        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;

        if (trueDamageAmount > 0f)
        {
            OnDamaged?.Invoke(trueDamageAmount);
        }
        HandleDeath();
        
    }

    /*
    public void InflictDamage(float damage, bool isExplosionDamage, Bullet bullet)
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
    */

    public void Dead()
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
    public bool PlayerDead()
    {
        return IsDead;
    }
}
