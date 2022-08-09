using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemAtPos : MonoBehaviour
{
    public ItemStats CurrentItem;
    void Start()
    {
        Dropitem();
    }
    private void Update()
    {
        if (CurrentItem != null)
        {
            Destroy(gameObject);
        }
    }
    void Dropitem()
    {
        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        Buff buff = ItemManager.Instance.GetBuff();
        ItemManager.Instance.DropWeaponAtPlace(RandomWeapon.id, buff.id, base.transform.position);
        CurrentItem = RandomWeapon;
    }
}
