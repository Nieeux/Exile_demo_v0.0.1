using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float DamageMultiplier = 1f;

    public Health Health { get; private set; }

    void Awake()
    {
        // find the health component either at the same level, or higher in the hierarchy
        Health = GetComponent<Health>();
        if (!Health)
        {
            Health = GetComponentInParent<Health>();
        }
    }

    public void InflictDamage(float damage, bool isExplosionDamage)
    {
        if (Health)
        {
            var totalDamage = damage;

            // skip the crit multiplier if it's from an explosion
            if (!isExplosionDamage)
            {
                totalDamage *= DamageMultiplier;
            }

            // apply the damages
            Health.TakeDamage(totalDamage);
        }
    }
}
