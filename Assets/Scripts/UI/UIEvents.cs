using System;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
	public GameObject pickupPrefab;

	public Transform pickupParent;

	public static UIEvents Instance;

	private void Awake()
	{
		UIEvents.Instance = this;
	}

	private void Start()
	{

	}

	public void AddPickup(ItemStats item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.pickupPrefab, this.pickupParent);
		gameObject.GetComponent<PickedupUI>().SetItem(item);
		gameObject.transform.SetSiblingIndex(0);
	}

	public void PlaceInInventory(ItemStats item)
	{

	}
}
