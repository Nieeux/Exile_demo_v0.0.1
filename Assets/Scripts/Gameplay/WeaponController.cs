using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{

    public Transform firePoint;
    private PlayerWeaponManager Recoil;
    public HelmetScript RecoilHelmet;
    public bool singleFire = false;

    public LayerMask enemy;

    public AudioClip fireAudio;
    public AudioClip reloadAudio;

    [SerializeField] private InventoryItem GunStats;

    float nextFireTime = 0;
    public bool canFire = true;

    AudioSource audioSource;

    private Player player;

    void Awake()
    {
        this.player = base.GetComponentInParent<Player>();
    }
    void Start()
    {
        GunStats.CurrentMagazine = GunStats.Magazine;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //Make sound 3D
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
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
                    GameObject bulletObject = Instantiate(GunStats.bulletPrefab, firePoint.position, firePoint.rotation);
                    WeaponAnimation.Instance.RecoilAni();
                    PlayerWeaponManager.Instance.RecoilFire();
                    this.RecoilHelmet.RecoilHUD();

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
}
