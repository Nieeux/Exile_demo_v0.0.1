using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStatsInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public multiLanguage language;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemInfo.Instance.SetText(language.GetLanguage());
        ItemInfo.Instance.SetWeight(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfo.Instance.OnDisable();
    }
}
