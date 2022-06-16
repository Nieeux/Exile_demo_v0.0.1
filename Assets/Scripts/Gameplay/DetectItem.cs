using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

// Token: 0x02000033 RID: 51
public class DetectItem : MonoBehaviour
{
	public LayerMask whatIsInteractable;
	public GameObject interactUI;
	private Transform interactUi;
	private TextMeshProUGUI interactText;
	public Transform playerCamera;
	public static DetectItem Instance;
	private Collider currentCollider;

	private void Awake()
	{
		DetectItem.Instance = this;
		this.interactUi = UnityEngine.Object.Instantiate<GameObject>(this.interactUI).transform;
		this.interactText = this.interactUi.GetComponentInChildren<TextMeshProUGUI>();
		this.interactUi.gameObject.SetActive(false);
	}

	public Interactable currentInteractable { get; private set; }

	private void Update()
	{
		RaycastHit raycastHit;
		if (Physics.SphereCast(this.playerCamera.position, 1.5f, this.playerCamera.forward, out raycastHit, 4f, this.whatIsInteractable))
		{

			// di vao trigger hien ten item
			if (raycastHit.collider.isTrigger)
			{
				this.currentInteractable = raycastHit.collider.gameObject.GetComponent<Interactable>();
				if (this.currentInteractable == null)
				{
					return;
				}
				if (this.currentInteractable != null)
				{
					this.currentCollider = raycastHit.collider;
				}
				this.interactUi.gameObject.SetActive(true);
				this.interactText.text = (this.currentInteractable.GetName() ?? "");
				this.interactUi.transform.position = raycastHit.collider.gameObject.transform.position + Vector3.up * raycastHit.collider.bounds.extents.y;
				this.interactText.CrossFadeAlpha(1f, 0.1f, false);
				return;
			}
		}
		else
		{
			this.currentCollider = null;
			this.currentInteractable = null;
			this.interactText.CrossFadeAlpha(0f, 0.1f, false);
		}
	}

}
