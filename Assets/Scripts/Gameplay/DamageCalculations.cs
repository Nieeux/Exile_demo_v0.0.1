using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculations : MonoBehaviour
{
    public static DamageCalculations Instance;

    private static Vector2 randomDamageRange = new Vector2(0.7f, 1f);

    private void Awake()
    {
        DamageCalculations.Instance = this;
    }
    public DamageCalculations.DamageResult GetDamage()
    {
        float dmg = Random.Range(DamageCalculations.randomDamageRange.x, DamageCalculations.randomDamageRange.y);
        bool crit = Random.Range(0f, 1f) < 0.1f;
        if (crit)
        {
            dmg *= 2f;
        }
        return new DamageCalculations.DamageResult(dmg, crit);
    }


    public class DamageResult
    {
        public float damageMultiplier;
        public bool crit;
        public float AmmoPiercing;

        public DamageResult(float damage, bool crit)
        {
            this.damageMultiplier = damage;
            this.crit = crit;
            //this.AmmoPiercing = AmmoPiercing;
        }
    }
}
