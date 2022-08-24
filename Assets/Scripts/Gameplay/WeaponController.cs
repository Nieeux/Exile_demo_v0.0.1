using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;

    public Transform firePoint;
    public bool singleFire = false;

    public AudioClip fireAudio;
    public AudioClip reloadAudio;

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
    public AudioClip ChangeWeaponSfx;

    public Bullet Pullet;

    public int WeaponID;
    public Interact WeaponIndex { get; private set; }

    public bool IsWeaponActive { get; private set; }
    public bool IsCharging { get; private set; }

    void Awake()
    {
        WeaponController.Instance = this;

        //GunStats.CurrentMagazine = GunStats.Magazine;
    }
    void Start()
    {
        this.WeaponIndex = GetComponent<PickupWeapon>();
        rb = base.GetComponent<Rigidbody>();
        coll = base.GetComponent<BoxCollider>();

        Bullet bullet = GunStats.bulletPrefab.GetComponent<Bullet>();
        WeaponAmmoType = bullet.ammoTypeColor;
        this.Pullet = bullet;

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {

        if (canFire == true)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
                canDrop = false;
                if (delay <= 0)
                {
                    
                    delay = 0;
                }
            }
            if(delay == 0)
            {
                canDrop = true;
            }
            if (PlayerInput.Instance.GetFireButtonDown() && singleFire)
            {
                Fire();
                delay = 0.25f;
            }

            if (PlayerInput.Instance.GetFireButton() && !singleFire)
            {
                Fire();
                delay = 0.25f;
            }
            if (Input.GetKeyDown(KeyCode.R) && !reloading && GunStats.CurrentMagazine < GunStats.Magazine)
            {
                StartCoroutine(Reload(true));
            }
        }

        /*
        // Check Enemy đã vào tầm chưa?
        enemyInRange = Physics.CheckSphere(transform.position, range, enemy);
        // Check Player đã thấy Enemy chưa?
        seeEnemy = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, 100, enemy));

        if (enemyInRange && seeEnemy) Fire();

        if (Input.GetKeyDown(KeyCode.R) && canFire)
        {
            StartCoroutine(Reload());
        }
        */

    }
    private void Fire()
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
                        for(int i = 0; i < Pullet.bulletsPerShot; i++)
                        {
                            Vector3 direction = ShotgunDirection();
                            RaycastHit hit;
                            Vector3 firePointPointerPosition = firePoint.transform.position + firePoint.transform.forward * 100;

                            if (Physics.Raycast(firePoint.transform.position, direction, out hit, 100))
                            {
                                firePointPointerPosition = hit.point;
                            }
                            firePoint.LookAt(firePointPointerPosition);

                            Bullet bulletobject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                            bulletobject.BulletDamage = GunStats.GunDamage;
                            this.WeaponAmmoType = bulletobject.ammoTypeColor;
                        }
                    }
                    else
                    {
                        Vector3 firePointPointerPosition = firePoint.transform.position + firePoint.transform.forward * 100;

                        RaycastHit hit;
                        if (Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, 100))
                        {
                            firePointPointerPosition = hit.point;
                        }
                        firePoint.LookAt(firePointPointerPosition);
                        //Fire
                        Bullet bulletObject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                        bulletObject.BulletDamage = GunStats.GunDamage;
                        this.WeaponAmmoType = bulletObject.ammoTypeColor;
                       
                    }
                    CameraShake.Instance.Shake();
                   
                    // giam Durability khi ban
                    RecoilAni();

                    GunStats.CurrentMagazine--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();

                    GunStats.CurrentDurability -= 0.5f;
                    if (GunStats.CurrentDurability <= 0)
                    {
                        canFire = false;
                        base.Invoke("BrokenWeapon", 1f);

                    }
                    WeaponUIManager.Instance.updateUI(WeaponID);
                }
                else
                {
                    StartCoroutine(Reload(true));
                }
            }
        }
    }
    private void RecoilAni()
    {
        //Súng gi?t
        Sequence s = DOTween.Sequence();
        s.Append(base.transform.DOShakeRotation(GunStats.ShakeDuration, GunStats.ShakeStrenght, GunStats.punchVibrato, GunStats.randomness, true));
        Sequence r = DOTween.Sequence();
        r.Append(base.transform.DOPunchRotation(GunStats.PunchR, GunStats.punchDurationR, GunStats.punchVibrato, GunStats.punchElasticity));
        Sequence p = DOTween.Sequence();
        p.Append(base.transform.DOPunchPosition(new Vector3(0, 0, -GunStats.punchStrenght), GunStats.punchDuration, GunStats.punchVibrato, GunStats.punchElasticity));

    }

    private void BrokenWeapon()
    {
        WeaponInventory.Instance.BrokeWeapon(this, GunStats);

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

                            Bullet bulletobject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
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
                        Bullet bulletObject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                        bulletObject.BulletDamage = GunStats.GunDamage;
                    }

                    // giam Durability khi ban
                    GunStats.CurrentDurability -= 0.5f;
                    //UIBob.Instance.RecoilHUD();

                    GunStats.CurrentMagazine--;
                    audioSource.clip = fireAudio;
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

        audioSource.PlayOneShot(reloadAudio);
        audioSource.minDistance = 1;


        yield return new WaitForSeconds(GunStats.ReloadTime);

        GunStats.CurrentMagazine = GunStats.Magazine;
        audioSource.minDistance = 10;
        reloading = !Reloading;
    }

    private IEnumerator Reload(bool Reloading)
    {
        reloading = Reloading;

        audioSource.PlayOneShot(reloadAudio);
        audioSource.minDistance = 1;
        WeaponUIManager.Instance.updateUI(WeaponID);

        yield return new WaitForSeconds(GunStats.ReloadTime);

        GunStats.CurrentMagazine = GunStats.Magazine;
        audioSource.minDistance = 10;
        reloading = !Reloading;
        WeaponUIManager.Instance.updateUI(WeaponID);
    }

    public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);
        canFire = show;
        //enabled = (show);
        if (show && ChangeWeaponSfx)
        {
            audioSource.PlayOneShot(ChangeWeaponSfx);
        }

        IsWeaponActive = show;
    }
}
