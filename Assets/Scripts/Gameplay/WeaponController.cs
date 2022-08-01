using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;

    public Transform firePoint;
    public bool singleFire = false;

    public AudioClip fireAudio;
    public AudioClip reloadAudio;

    public ItemStats GunStats;

    float nextFireTime = 0;
    public bool reloading;
    public bool canFire = false;
    public bool AiEquip = false;


    public Rigidbody rb;
    public BoxCollider coll;

    AudioSource audioSource;

    public GameObject WeaponRoot;
    public AudioClip ChangeWeaponSfx;
    AudioSource m_ShootAudioSource;

    public UnityAction onDamaged;
    public Interactable WeaponIndex { get; private set; }

    public bool IsWeaponActive { get; private set; }
    public bool IsCharging { get; private set; }
    public GameObject Owner { get; set; }
    public GameObject SourcePrefab { get; set; }

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


        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //Make sound 3D
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {

        if (canFire == true && AiEquip == false)
        {

            if (PlayerInput.Instance.GetFireButtonDown() && singleFire)
            {
                Fire();

            }
            if (PlayerInput.Instance.GetFireButton() && !singleFire)
            {
                Fire();
            }
            if (Input.GetKeyDown(KeyCode.R) && canFire && GunStats.CurrentMagazine != GunStats.Magazine)
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
    void CalculateStats()
    {
        //foreach(InventoryItem GunStats in GunStats)

    }
    private void Fire()
    {
        if (canFire)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + GunStats.fireRate;

                if (GunStats.CurrentMagazine > 0)
                {
                    //Point fire point at the current center of Camera
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
                    bulletObject.BulletCrit = GunStats.Critical;

                    // giam Durability khi ban
                    RecoilAni();


                    PlayerWeaponManager.Instance.RecoilFire();
                    //UIBob.Instance.RecoilHUD();

                    GunStats.CurrentMagazine--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();

                    GunStats.CurrentDurability -= 0.5f;
                    if (GunStats.CurrentDurability <= 0)
                    {
                        canFire = false;
                        base.Invoke("BrokenWeapon", 1f);

                    }
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
        s.Append(transform.DOShakeRotation(GunStats.ShakeDuration, GunStats.ShakeStrenght, GunStats.punchVibrato, GunStats.randomness, true));
        Sequence r = DOTween.Sequence();
        r.Append(transform.DOPunchRotation(new Vector3(-20, 0, -10), GunStats.punchDurationR, GunStats.punchVibrato, GunStats.punchElasticity));
        Sequence p = DOTween.Sequence();
        p.Append(transform.DOPunchPosition(new Vector3(0, 0, -GunStats.punchStrenght), GunStats.punchDuration, GunStats.punchVibrato, GunStats.punchElasticity));

    }
    private void BrokenWeapon()
    {
        PlayerWeaponManager.Instance.BrokeWeapon(this, GunStats);
    }
    public void AiFire()
    {
        if (canFire)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + GunStats.fireRate;

                if (GunStats.CurrentMagazine > 0)
                {
                    float inaccuracy = 0.2f;
                    Vector3 firePointPointerPosition = firePoint.transform.position + firePoint.transform.forward * 100;

                    firePointPointerPosition += Random.insideUnitSphere * inaccuracy;
                    firePoint.LookAt(firePointPointerPosition);
                    //Fire
                    Bullet bulletObject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                    bulletObject.BulletDamage = GunStats.GunDamage;
                    bulletObject.BulletCrit = GunStats.Critical;
                    // giam Durability khi ban
                    GunStats.CurrentDurability -= 0.5f;
                    //UIBob.Instance.RecoilHUD();

                    GunStats.CurrentMagazine--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();
                }
                else
                {
                    StartCoroutine(Reload(true));
                }
            }
        }
    }
    IEnumerator Reload(bool Reloading)
    {
        reloading = Reloading;
        canFire = !Reloading;

        audioSource.clip = reloadAudio;
        audioSource.Play();

        yield return new WaitForSeconds(GunStats.ReloadTime);

        GunStats.CurrentMagazine = GunStats.Magazine;
        reloading = !Reloading;
        canFire = Reloading;
    }

    public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);
        canFire = show;
        //enabled = (show);
        if (show && ChangeWeaponSfx)
        {
            m_ShootAudioSource.PlayOneShot(ChangeWeaponSfx);
        }

        IsWeaponActive = show;
    }
}
