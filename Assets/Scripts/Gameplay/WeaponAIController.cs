using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class WeaponAIController : WeaponController
{
    void Start()
    {
        base.WeaponIndex = GetComponent<PickupWeapon>();
        rb = base.GetComponent<Rigidbody>();
        coll = base.GetComponent<BoxCollider>();

        coll.enabled = false;
        rb.isKinematic = true;

        WeaponAmmoType = GunStats.bulletType.ammoTypeColor;
        this.Pullet = GunStats.bulletType;

        audioSource = GetComponent<AudioSource>();

    } 

    public void AiFire()
    {
        if (!reloading)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + GunStats.fireRate;

                if (GunStats.CurrentMagazine > 0)
                {
                    if (this.Pullet.IsShotGunShell == true)
                    {
                        for (int i = 0; i < Pullet.bulletsPerShot; i++)
                        {
                            Vector3 direction = ShotgunDirection();
                            RaycastHit hit;
                            Vector3 firePointPointerPosition = firePoint.transform.position + firePoint.transform.forward * 100;

                            if (Physics.Raycast(firePoint.transform.position, direction, out hit, 100))
                            {
                                firePointPointerPosition = hit.point;
                            }
                            firePoint.LookAt(firePointPointerPosition);

                            Bullet bulletobject = Instantiate(GunStats.bulletType.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                            bulletobject.BulletDamage = GunStats.GunDamage;
                            this.WeaponAmmoType = bulletobject.ammoTypeColor;
                        }
                    }
                    else
                    {
                        Vector3 direction = GetDirection();
                        Vector3 firePointPointerPosition = firePoint.transform.position + firePoint.transform.forward * 100;

                        RaycastHit hit;
                        if (Physics.Raycast(firePoint.transform.position, direction, out hit, 100))
                        {
                            firePointPointerPosition = hit.point;
                        }
                        firePoint.LookAt(firePointPointerPosition);
                        //Fire
                        Bullet bulletObject = Instantiate(GunStats.bulletType.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                        bulletObject.BulletDamage = GunStats.GunDamage;
                    }


                    GunStats.CurrentDurability -= 0.5f;


                    GunStats.CurrentMagazine--;
                    audioSource.clip = GunStats.fireAudio;
                    audioSource.Play();
                }
                else
                {
                    StartCoroutine(AiReload(true));
                }
            }
        }

    }
    
    private IEnumerator AiReload(bool Reloading)
    {
        reloading = Reloading;

        audioSource.PlayOneShot(GunStats.reloadAudio);
        audioSource.minDistance = 1;


        yield return new WaitForSeconds(GunStats.ReloadTime);

        GunStats.CurrentMagazine = GunStats.Magazine;
        audioSource.minDistance = 10;
        reloading = !Reloading;
    }
}
