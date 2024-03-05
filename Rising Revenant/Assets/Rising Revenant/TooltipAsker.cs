using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for UI event handling

public class TooltipAsker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;
    public TooltipManager.TooltipPosition position;
    public float margin = 10f; // Margin distance, you can adjust this value in the Inspector
    public bool follow = false; 
    public GameObject tooltipPrefab; // Assign in inspector
    public bool enable = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enable) return;
        UiEntitiesReferenceManager.tooltipManager.ShowTooltip(tooltipPrefab,message, GetComponent<RectTransform>(), position, margin, follow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enable) return;
        UiEntitiesReferenceManager.tooltipManager.HideTooltip();
    }
}
