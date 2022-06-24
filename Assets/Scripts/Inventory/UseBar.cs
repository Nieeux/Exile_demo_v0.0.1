using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBar : MonoBehaviour
{
    public static UseBar Instance;
    private InventoryItem currentItem;

    private void Awake()
    {
        UseBar.Instance = this;
        this.SetWeapon(null);
    }

    public void SetWeapon(InventoryItem item)
    {
        //this.StopUse();
        this.currentItem = item;
        if (item == null)
        {
            return;
        }
        GameObject Item = Instantiate(currentItem.prefab,transform);
        Item.GetComponent<Rigidbody>().isKinematic = true;
        transform.localRotation = Quaternion.Euler(item.rotationOffset);
        //transform.localScale = Vector3.one * item.scale;
        transform.localPosition = item.positionOffset;
    }

    private void StopUse()
    {
        base.CancelInvoke();
    }

}
