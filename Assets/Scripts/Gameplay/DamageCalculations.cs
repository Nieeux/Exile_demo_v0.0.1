using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

public class DamageCalculations : MonoBehaviour
{
    public static DamageCalculations Instance;

    private static Vector2 randomDamageRange = new Vector2(0.8f, 1f);

    private void Awake()
    {
        DamageCalculations.Instance = this;
    }

    public class DamageResult
    {
        public float damageResult;
        public bool ItCrit;

        public DamageResult(float damage, bool crit)
        {
            this.damageResult = damage;
            this.ItCrit = crit;

        }
    }
    public class ArmorResult
    {
        public float damageResult;
        public HitEffect.HitType hitType;

        public ArmorResult(float damage, HitEffect.HitType hitEffect)
        {
            this.damageResult = damage;
            this.hitType = hitEffect;

        }
    }

    public DamageCalculations.DamageResult GetAiDamage(Bullet ammoType)
    {

        float dmg = Random.Range(DamageCalculations.randomDamageRange.x, DamageCalculations.randomDamageRange.y);
        bool ItCrit = Random.value < 0.1f;

        if (ammoType.ammoType == Bullet.AmmoType.NormalAmmo)
        {
            dmg *= 1.2f;
            Debug.Log("HitNormalAmmo");

        }
        if (ammoType.ammoType == Bullet.AmmoType.PiercingAmmo)
        {
            dmg *= 1.5f;
            Debug.Log("HitPiercingAmmo");
        }
        if (ammoType.ammoType == Bullet.AmmoType.HighAmmo)
        {
            dmg *= 2;
            Debug.Log("HitHighAmmo");
        }
        if (ItCrit)
        {
            dmg *= 2f;
        }

        return new DamageCalculations.DamageResult(dmg, ItCrit);
    }

    public DamageCalculations.DamageResult GetPlayerDamage(Bullet ammoType)
    {

        float dmg = Random.Range(DamageCalculations.randomDamageRange.x, DamageCalculations.randomDamageRange.y); 
        dmg *= Inventory.Instance.GetDamage();
        bool ItCrit = Random.value < Inventory.Instance.GetCritical();

        if (ammoType.ammoType == Bullet.AmmoType.NormalAmmo)
        {
            dmg *= 1.2f;
            Debug.Log("HitNormalAmmo");

        }
        if (ammoType.ammoType == Bullet.AmmoType.PiercingAmmo)
        {
            dmg *= 1.5f;
            Debug.Log("HitPiercingAmmo");
        }
        if (ammoType.ammoType == Bullet.AmmoType.HighAmmo)
        {
            dmg *= 2;
            Debug.Log("HitHighAmmo");
        }
        if (ItCrit)
        {
            dmg *= 2f;
        }

        return new DamageCalculations.DamageResult(dmg, ItCrit);
    }

    public DamageCalculations.ArmorResult GetDamageArmor(ItemStats Armor,Bullet bulletType,bool crit, float damage)
    {

        HitEffect.HitType Effect = HitEffect.HitType.Null;
        float dmg = damage;

        if (Armor.armorType == ItemStats.ArmorType.LightArmor)
        {
            if (bulletType.ammoType == Bullet.AmmoType.NormalAmmo)
            {
                dmg = damage;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.Normal;
                }
                
            }
            if (bulletType.ammoType == Bullet.AmmoType.PiercingAmmo)
            {
                dmg /= 1.3f;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.Low;
                }
                
            }
            if (bulletType.ammoType == Bullet.AmmoType.HighAmmo)
            {
                dmg *= 1.3f;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.High;
                }  
            }

        }
        if (Armor.armorType == ItemStats.ArmorType.NormalArmor)
        {
            if (bulletType.ammoType == Bullet.AmmoType.NormalAmmo)
            {
                dmg /= 1.3f;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.Low;
                }
            }
            if (bulletType.ammoType == Bullet.AmmoType.PiercingAmmo)
            {
                dmg = damage;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.High;
                }
            }
            if (bulletType.ammoType == Bullet.AmmoType.HighAmmo)
            {
                dmg /= 1.2f;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.Normal;
                }
            }

        }
        if (Armor.armorType == ItemStats.ArmorType.HeavyArmor)
        {
            if (bulletType.ammoType == Bullet.AmmoType.NormalAmmo)
            {
                dmg /= 1.5f;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.Low;
                }
            }
            if (bulletType.ammoType == Bullet.AmmoType.PiercingAmmo)
            {
                dmg = damage;
                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.High;
                }
            }
            if (bulletType.ammoType == Bullet.AmmoType.HighAmmo)
            {
                dmg /= 1.5f;

                if (crit)
                {
                    Effect = HitEffect.HitType.Critical;
                }
                else
                {
                    Effect = HitEffect.HitType.Low;
                }
            }

        }

        return new DamageCalculations.ArmorResult(dmg, Effect);
    }
}
