using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public Inventory inventory;
    public DetectItem detectItem;
    [Header("Inventory")]
    public GameObject TipUseItem;
    public GameObject TipSellItem;
    void Start()
    {
        
        TipUseItem.SetActive(false);
        TipSellItem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ActiveMenu())
        {
            Interact currentInteractable = DetectItem.Instance.currentInteractable;
            bool active = currentInteractable == null || currentInteractable != null && currentInteractable.IsStarted() == false;
            if (!inventory.ItemCurrentIsNull() && active)
            {
                TipUseItem.SetActive(true);
            }
            else
            {
                TipUseItem.SetActive(false);
            }
            if (currentInteractable != null)
            {
                if (!inventory.ItemCurrentIsNull() && detectItem.currentInteractable.IsStarted() == true)
                {
                    TipSellItem.SetActive(true);
                }
                else
                {
                    TipSellItem.SetActive(false);
                }
            }

        }
        else
        {
            TipSellItem.SetActive(false);
            TipUseItem.SetActive(false);
        }
    }
    public bool ActiveMenu()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
