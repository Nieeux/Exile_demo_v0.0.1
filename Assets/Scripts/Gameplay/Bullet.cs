using System.Collections;
using UnityEngine;
using System;


public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 345;
    public float hitForce = 50f;
    public float destroyAfter = 3.5f;

    public Bullet.AmmoType ammoType;
    public Color ammoTypeColor;
    public GameObject bulletLight;
    float currentTime = 0;
    public float BulletDamage;
    public float BulletCrit;
    Vector3 newPos;
    Vector3 oldPos;
    bool hasHit = false;
    Vector3 pos;

    void Awake()
    {

    }

    IEnumerator Start()
    {
        if (ammoType == AmmoType.HighAmmo)
        {
            BulletDamage *= 2f;
        }
        if (ammoType == AmmoType.PiercingAmmo)
        {
            BulletDamage *= 1.5f;
        }
        newPos = transform.position;
        oldPos = newPos;

        while (currentTime < destroyAfter && !hasHit)
        {
            Vector3 velocity = transform.forward * bulletSpeed;
            newPos += velocity * Time.deltaTime;
            Vector3 direction = newPos - oldPos;
            float distance = direction.magnitude;
            RaycastHit hit;

            // Check if we hit anything on the way
            if (Physics.Raycast(oldPos, direction, out hit, distance))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(direction * hitForce);
                }
                OnHit(hit.collider);

                newPos = hit.point; //Adjust new position
                StartCoroutine(DestroyBullet());
            }

            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();

            transform.position = newPos;
            oldPos = newPos;
        }

        if (!hasHit)
        {
            StartCoroutine(DestroyBullet());
        }
    }
    public void OnHit(Collider collider)
    {
        // Enemy gay damage cho Player
        if (collider.CompareTag("Player"))
        {
            PlayerStats damageablePlayer = collider.GetComponent<PlayerStats>();
            if (damageablePlayer)
            {
                DamageCalculations.DamageResult damageMultiplier = DamageCalculations.Instance.GetDamage();
                float damageMultiplier2 = damageMultiplier.damageMultiplier;
                float Damage = (int)(BulletDamage * damageMultiplier2);

                damageablePlayer.Damage(Damage);
                
            }
        }
        // Player gay damage cho Enemy
        HitAble damageable = collider.GetComponent<HitAble>();
        if (damageable)
        {
            Inventory.DamageResult damageMultiplier = Inventory.Instance.GetDamage();
            float damageMultiplier2 = damageMultiplier.damageMultiplier;
            bool crit = damageMultiplier.ItCrit;
            float Damage = (int)(BulletDamage * damageMultiplier2);
            HitEffect hitEffect = HitEffect.AmmoNormal;
            if (crit)
            {
                hitEffect = HitEffect.Crit;
            }
            pos = collider.transform.position;

            damageable.Damage(Damage, (int)hitEffect, pos);
        }
        Destroy(this.bulletLight);


    }
    IEnumerator DestroyBullet()
    {
        hasHit = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    [Serializable]
    public enum AmmoType
    {
        NormalAmmo,
        HighAmmo,
        PiercingAmmo,
    }
}
