using TMPro;
using UnityEngine;


// Token: 0x02000033 RID: 51
public class DetectItem : MonoBehaviour
{
    [Header("Detect Item")]

    public GameObject interactUI;
    public TextMeshProUGUI interactText;
    public static DetectItem Instance;
    public Color colorOriginal;
    public Color colorUpgrade;
    public Color colorAdvanced;

    public Interact currentInteractable { get; private set; }

    private void Awake()
	{
		DetectItem.Instance = this;
		this.interactUI.SetActive(false);
	}
    private void Start()
    {

    }
    private void Update()
    {

        RaycastHit Hit;
        if (Physics.Raycast(transform.position, transform.forward, out Hit, 2f) && Hit.transform.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            this.currentInteractable = Hit.collider.gameObject.GetComponent<Interact>();
            if (this.currentInteractable != null)
            {
                this.interactUI.SetActive(true);

                this.interactText.text = (this.currentInteractable.GetName() ?? "");

                if (currentInteractable.GetItem() != null)
                {
                    if (currentInteractable.GetItem().Rare == ItemStats.ItemRare.original)
                    {
                        this.interactText.color = colorOriginal;
                    }
                    if (currentInteractable.GetItem().Rare == ItemStats.ItemRare.upgrade)
                    {
                        this.interactText.color = colorUpgrade;
                    }
                    if (currentInteractable.GetItem().Rare == ItemStats.ItemRare.advanced)
                    {
                        this.interactText.color = colorAdvanced;
                    }
                }
                else
                {
                    this.interactText.color = colorOriginal;
                }

                this.interactText.CrossFadeAlpha(1f, 0.1f, false);
            }
        }
        else
        {
            if (this.currentInteractable != null)
            {
                this.currentInteractable = null;
                this.interactText.CrossFadeAlpha(0f, 0.1f, false);
            }

        }
    }
}
