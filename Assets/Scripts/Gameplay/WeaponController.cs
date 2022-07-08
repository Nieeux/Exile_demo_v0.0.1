﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource), typeof(ItemStatsGenerator))]
public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;

    public Transform firePoint;
    public bool singleFire = false;

    public AudioClip fireAudio;
    public AudioClip reloadAudio;

    public InventoryItem GunStats;
    ItemStatsGenerator StatsGenerator;

    float nextFireTime = 0;
    public bool canFire = true;
    public Rigidbody rb;
    public BoxCollider coll;

    AudioSource audioSource;

    private PlayerMovement player;

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
        
        this.player = base.GetComponentInParent<PlayerMovement>();
        //GunStats.CurrentMagazine = GunStats.Magazine;
    }
    void Start()
    {
        this.StatsGenerator = GetComponent<ItemStatsGenerator>();

        this.WeaponIndex = GetComponent<PickupWeapon>();
        rb = base.GetComponent<Rigidbody>();
        coll = base.GetComponent<BoxCollider>();
        WeaponController.Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //Make sound 3D
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
        if (GunStats == null)
        {
            GunStats = StatsGenerator.itemChange;
        }

        if (Input.GetMouseButtonDown(0) && singleFire)
        {
            Fire();
 
        }
        if (Input.GetMouseButton(0) && !singleFire)
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R) && canFire)
        {
            StartCoroutine(Reload());
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
    void Fire()
    {
        if (canFire)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + GunStats.fireRate;

                if (GunStats.CurrentMagazine > 0)
                {
                    //Point fire point at the current center of Camera
                    Vector3 firePointPointerPosition = player.playerCamera.transform.position + player.playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(player.playerCamera.transform.position, player.playerCamera.transform.forward, out hit, 100))
                    {
                        firePointPointerPosition = hit.point;
                    }
                    firePoint.LookAt(firePointPointerPosition);
                    //Fire
                    Bullet bulletObject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
                    bulletObject.DamageBullet = GunStats.GunDamage;
                    // giam Durability khi ban
                    GunStats.CurrentDurability -= 0.5f;

                    WeaponAnimation.Instance.RecoilAni();
                    PlayerWeaponManager.Instance.RecoilFire();
                    //HelmetScript.Instance.RecoilHUD();

                    GunStats.CurrentMagazine--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();
                }
                else
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }
    IEnumerator Reload()
    {
        canFire = false;

        audioSource.clip = reloadAudio;
        audioSource.Play();

        yield return new WaitForSeconds(GunStats.ReloadTime);

        GunStats.CurrentMagazine = GunStats.Magazine;

        canFire = true;
    }

    public void ShowWeapon(bool show)
    {
        WeaponRoot.SetActive(show);
        //enabled = (show);
        if (show && ChangeWeaponSfx)
        {
            m_ShootAudioSource.PlayOneShot(ChangeWeaponSfx);
        }

        IsWeaponActive = show;
    }
}
