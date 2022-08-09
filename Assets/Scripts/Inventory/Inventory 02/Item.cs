using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item Instance;
    public Interact ItemIndex { get; private set; }
    public bool IsCharging { get; private set; }
    public bool IsWeaponActive { get; private set; }
    public GameObject Owner { get; set; }
    public GameObject SourcePrefab { get; set; }

    public ItemStats ItemStats;

    public Rigidbody rb;
    public BoxCollider coll;

    public GameObject WeaponRoot;
    public AudioClip ChangeWeaponSfx;
    AudioSource m_ShootAudioSource;

    private void Awake()
    {
        Item.Instance = this;
    }
    void Start()
    {
        this.ItemIndex = GetComponent<PickupItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowItem(bool show)
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
