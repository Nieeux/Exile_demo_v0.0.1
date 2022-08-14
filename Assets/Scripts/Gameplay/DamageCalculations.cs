using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculations : MonoBehaviour
{
    public class DamageResult
    {
        public float damageResult;
        public bool ItCrit;
        public float AmmoPiercing;

        public DamageResult(float damage, bool crit)
        {
            this.damageResult = damage;
            this.ItCrit = crit;

        }
    }
    public class ArmorResult
    {
        public float damageResult;

        public ArmorResult(float damage)
        {
            this.damageResult = damage;

        }
    }

    public static DamageCalculations Instance;

    private static Vector2 randomDamageRange = new Vector2(0.7f, 1f);

    public static float Criterate;

    private void Awake()
    {
        DamageCalculations.Instance = this;
    }

    public DamageCalculations.DamageResult GetDamage(Bullet ammoType)
    {
        float dmg = Random.Range(DamageCalculations.randomDamageRange.x, DamageCalculations.randomDamageRange.y);
        bool ItCrit = Random.Range(0f, 1f) < 0.1f;

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

    public DamageCalculations.ArmorResult GetDamageArmor(ItemStats Armor,Bullet bulletType, float damage)
    {
        float dmg = damage;

        if (Armor.armorType == ItemStats.ArmorType.LightArmor)
        {
            if (bulletType.ammoType == Bullet.AmmoType.NormalAmmo)
            {
                dmg /= 1.2f;
            }
            if (bulletType.ammoType == Bullet.AmmoType.PiercingAmmo)
            {
                dmg /= 1.2f;
            }
            if (bulletType.ammoType == Bullet.AmmoType.HighAmmo)
            {
                dmg /= 1.1f;
            }
            Debug.Log("HitLightArmor");
        }
        if (Armor.armorType == ItemStats.ArmorType.NormalArmor)
        {
            if (bulletType.ammoType == Bullet.AmmoType.NormalAmmo)
            {
                dmg /= 1.5f;
            }
            if (bulletType.ammoType == Bullet.AmmoType.PiercingAmmo)
            {
                dmg /= 1.2f;
            }
            if (bulletType.ammoType == Bullet.AmmoType.HighAmmo)
            {
                dmg /= 1.1f;
            }
            Debug.Log("HitNormalArmor");
        }
        if (Armor.armorType == ItemStats.ArmorType.HeavyArmor)
        {
            if (bulletType.ammoType == Bullet.AmmoType.NormalAmmo)
            {
                dmg /= 2f;
            }
            if (bulletType.ammoType == Bullet.AmmoType.PiercingAmmo)
            {
                dmg /= 1.1f;
            }
            if (bulletType.ammoType == Bullet.AmmoType.HighAmmo)
            {
                dmg /= 2f;
            }
            Debug.Log("HitHeavyArmor");
        }

        return new DamageCalculations.ArmorResult(dmg);
    }
}
