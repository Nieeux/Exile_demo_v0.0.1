using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculations : MonoBehaviour
{
    public static DamageCalculations Instance;

    private static Vector2 randomDamageRange = new Vector2(0.7f, 1f);

    public static float Criterate;

    private void Awake()
    {
        DamageCalculations.Instance = this;
    }
    public void Update()
    {
        
    }
    public DamageCalculations.DamageResult GetDamage()
    {
        float dmg = Random.Range(DamageCalculations.randomDamageRange.x, DamageCalculations.randomDamageRange.y);
        bool ItCrit = Random.Range(0f, 1f) < EquipAble.Instance.Critical();

        if (ItCrit)
        {
            dmg *= 2f;
        }
        return new DamageCalculations.DamageResult(dmg, ItCrit);
    }


    public class DamageResult
    {
        public float damageMultiplier;     
        public bool ItCrit;
        public float AmmoPiercing;

        public DamageResult(float damage, bool crit)
        {
            this.damageMultiplier = damage;
            this.ItCrit = crit;

            //this.AmmoPiercing = AmmoPiercing;
        }
    }
}
