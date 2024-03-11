using UnityEngine;
using UnityEngine.EventSystems; // Required for UI event handling
using System; // Needed for defining events

public class TooltipAsker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string message;
    public TooltipManager.TooltipPosition position;
    public Vector2 margin = new Vector2(10f, 10f); 
    public bool follow = false;
    public GameObject tooltipPrefab; 
    public bool show = true;
    public bool allowKeep = false;
    public bool keep = false; 

    public GameObject currentTooltipObj;

    // Define events for other classes to subscribe to
    public event Action<GameObject> OnTooltipShown;
    public event Action<GameObject> OnTooltipHidden;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!show) return;
        currentTooltipObj = UiEntitiesReferenceManager.tooltipManager.ShowTooltip(tooltipPrefab, message, GetComponent<RectTransform>(), position, margin, follow);
        OnTooltipShown?.Invoke(currentTooltipObj); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!show || keep) return; 
        UiEntitiesReferenceManager.tooltipManager.HideTooltip();
        OnTooltipHidden?.Invoke(currentTooltipObj);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!show || !allowKeep) return;
        if (keep) 
        {
            UiEntitiesReferenceManager.tooltipManager.HideTooltip();
            keep = false; 
            OnTooltipHidden?.Invoke(currentTooltipObj); 
        }
        else
        {
            keep = true; 
        }
    }
}
