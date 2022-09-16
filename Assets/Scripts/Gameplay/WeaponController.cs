using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;

    public Transform firePoint;
    public bool singleFire = false;

    public ItemStats GunStats;
    public Color WeaponAmmoType;

    public float nextFireTime = 0;
    public bool reloading;
    public float delay;
    public bool canDrop;
    public bool canFire = false;

    private Vector3 BulletSpreadVariance = new Vector3(0.01f, 0.01f, 0.01f);
    private Vector3 BulletShotGun = new Vector3(0.1f, 0.1f, 0.1f);

    public Rigidbody rb;
    public BoxCollider coll;

    public AudioSource audioSource { get; protected set; }

    public GameObject WeaponRoot;

    public Bullet Pullet;
    public GameObject Crosshair;

    public int WeaponID;
    public Interact WeaponIndex { get; protected set; }
    PickupWeapon pickup;

    public bool IsWeaponActive { get; protected set; }
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
        pickup = base.GetComponent<PickupWeapon>();



        coll.enabled = false;
        rb.isKinematic = true;


        WeaponAmmoType = GunStats.bulletType.ammoTypeColor;
        this.Pullet = GunStats.bulletType;

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

                            Bullet bulletobject = Instantiate(GunStats.bulletType.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
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
                        Bullet bulletObject = Instantiate(GunStats.bulletType.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                        bulletObject.BulletDamage = GunStats.GunDamage;
                        this.WeaponAmmoType = bulletObject.ammoTypeColor;
                       
                    }
                    CameraShake.Instance.Shake();
                   
                    // giam Durability khi ban
                    RecoilAni();

                    GunStats.CurrentMagazine--;
                    //audioSource.clip = GunStats.fireAudio;
                    //audioSource.Play();
                    audioSource.PlayOneShot(GunStats.fireAudio);

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

    protected Vector3 GetDirection()
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
    protected Vector3 ShotgunDirection()
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

    private IEnumerator Reload(bool Reloading)
    {
        reloading = Reloading;

        audioSource.PlayOneShot(GunStats.reloadAudio);
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

        IsWeaponActive = show;
    }
}
