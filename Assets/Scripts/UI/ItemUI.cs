using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
    public CanvasGroup CanvasGroup;
    public Image WeaponImage;
    private bool ActiveWeapon;
    public Item item;
    public GameObject SelectItem;
    [Tooltip("Scale when weapon not selected")]
    public Vector3 UnselectedScale = Vector3.one * 0.8f;
    [Range(0, 1)]
    [Tooltip("Opacity when weapon not selected")]
    public float UnselectedOpacity = 0.5f;

    public TextMeshProUGUI WeaponName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        bool isActiveWeapon = item == InventoryItem.Instance.GetActiveItem();
        CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, isActiveWeapon ? 1f : UnselectedOpacity, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, isActiveWeapon ? Vector3.one : UnselectedScale, Time.deltaTime * 10);
        SelectItem.SetActive(isActiveWeapon);

    }
    public void Initialize(Item i)
    {
        item = i;
        WeaponImage.sprite = i.ItemStats.sprite;
        WeaponName.text = i.ItemStats.name;

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfo.Instance.SetText(item.ItemStats.name + "\n<size=70%>" + item.ItemStats.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.OnDisable();
    }
}
