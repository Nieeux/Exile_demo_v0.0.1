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
            //Destroy(gameObject);
        }
    }
    void Dropitem()
    {

        ItemStats RandomWeapon = ItemManager.Instance.GetRandomWeapons();
        Buff buff = ItemManager.Instance.GetBuff();
        Buff Debuff = ItemManager.Instance.GetDeBuff();
        ItemManager.Instance.DropWeaponAtPlace(RandomWeapon.id, buff.id, Debuff.id, base.transform.position);
        CurrentItem = RandomWeapon;
    }
}
