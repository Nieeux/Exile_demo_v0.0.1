using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public GameObject Vending;
    public bool Enter;
    private void Start()
    {
        Vending.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag ==("Player"))
        {
            Vending.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            Debug.Log("Exit");
            Vending.SetActive(false);
        }

    }
}
