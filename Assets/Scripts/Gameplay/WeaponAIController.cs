using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class WeaponAIController : MonoBehaviour
{

    public Transform firePoint;
    public bool singleFire = false;

    public ItemStats GunStats;
    public Color WeaponAmmoType;

    float nextFireTime = 0;
    public bool reloading;
    public float delay;
    public bool canDrop;
    public bool canFire = false;

    private Vector3 BulletSpreadVariance = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 BulletShotGun = new Vector3(0.1f, 0.1f, 0.1f);

    public Rigidbody rb;
    public BoxCollider coll;

    AudioSource audioSource;

    public GameObject WeaponRoot;

    public Bullet Pullet;
    public GameObject Crosshair;

    public int WeaponID;
    public Interact WeaponIndex { get; private set; }

    public bool IsWeaponActive { get; private set; }
    public bool IsCharging { get; private set; }

    void Start()
    {
        this.WeaponIndex = GetComponent<PickupWeapon>();
        rb = base.GetComponent<Rigidbody>();
        coll = base.GetComponent<BoxCollider>();

        coll.enabled = false;
        rb.isKinematic = true;

        WeaponAmmoType = GunStats.bulletType.ammoTypeColor;
        this.Pullet = GunStats.bulletType;

        audioSource = GetComponent<AudioSource>();

    }

    private void BrokenWeapon()
    {
        //WeaponInventory.Instance.BrokeWeapon(this, GunStats);

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

                    // giam Durability khi ban
                    GunStats.CurrentDurability -= 0.5f;
                    //UIBob.Instance.RecoilHUD();

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

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        direction += new Vector3(
            Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
            Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
            Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
        );

        direction.Normalize();

        return direction;
    }
    private Vector3 ShotgunDirection()
    {
        Vector3 direction = transform.forward;

        direction += new Vector3(
            Random.Range(-BulletShotGun.x, BulletShotGun.x),
            Random.Range(-BulletShotGun.y, BulletShotGun.y),
            Random.Range(-BulletShotGun.z, BulletShotGun.z)
        );

        direction.Normalize();

        return direction;
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

    public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);
        canFire = show;
        //enabled = (show);
        if (show && GunStats.ChangeWeaponAudio)
        {
            audioSource.PlayOneShot(GunStats.ChangeWeaponAudio);
        }

        IsWeaponActive = show;
    }
}
