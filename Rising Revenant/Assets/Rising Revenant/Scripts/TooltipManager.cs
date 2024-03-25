using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private GameObject currentTooltip;
    private RectTransform canvasRectTransform;

    private bool liveDeltaMove = false;
    private RectTransform currentTargetUIElement;
    private TooltipPosition currentPosition;
    private TooltipAsker currentAsker;
    private Vector2 currentMargin; // Changed to Vector2 to support both X and Y margins

    public enum TooltipPosition
    {
        Above,
        Below,
        Left,
        Right,
        Center
    }

    private void Awake()
    {
        UiEntitiesReferenceManager.tooltipManager = this;
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public GameObject ShowTooltip(GameObject tooltipPrefab, string message, RectTransform targetUIElement, TooltipPosition position, Vector2 margin, bool liveDeltaMove, TooltipAsker tooltipAsker)
    {

        if (currentAsker != null)
        {
            currentAsker.isCurrentlyPermanent = false;
        }

        this.liveDeltaMove = liveDeltaMove;
        this.currentTargetUIElement = targetUIElement;
        this.currentPosition = position;
        this.currentMargin = margin; 
        this.currentAsker = tooltipAsker;

        if (currentTooltip != null) Destroy(currentTooltip);

        currentTooltip = Instantiate(tooltipPrefab, canvasRectTransform);
        currentTooltip.GetComponentInChildren<TMP_Text>().text = message;

        UpdateTooltipPosition(); 

        return currentTooltip;
    }

    public void HideTooltip()
    {
        if (currentTooltip != null) 
        {
            //if (currentAsker != null)
            //{
            //    Debug.Log("Hiding main call tooltip");
            //    Debug.Log
            //    currentAsker.isCurrentlyPermanent = false;
            //}

            Destroy(currentTooltip);
        }
    }

    private void Update()
    {
        if (liveDeltaMove)
        {
            UpdateTooltipPosition();
        }
    }

    private void UpdateTooltipPosition()
    {
        if (currentTooltip == null || currentTargetUIElement == null) return;

        Vector2 targetPosition = GetTooltipPosition(currentTargetUIElement, currentPosition, currentMargin);
        currentTooltip.GetComponent<RectTransform>().anchoredPosition = targetPosition;
    }

    private Vector2 GetTooltipPosition(RectTransform targetUIElement, TooltipPosition position, Vector2 margin)
    {
        Canvas canvas = targetUIElement.GetComponentInParent<Canvas>();
        Vector3[] corners = new Vector3[4];
        targetUIElement.GetWorldCorners(corners);
        Vector3 targetPosition = Vector3.zero;

        foreach (var corner in corners)
        {
            targetPosition += corner;
        }
        targetPosition /= corners.Length;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, targetPosition);
        Vector2 tooltipSize = currentTooltip.GetComponent<RectTransform>().sizeDelta;
        Vector2 adjustment = Vector2.zero;

        switch (position)
        {
            case TooltipPosition.Above:
                adjustment = new Vector2(margin.x, targetUIElement.rect.height / 2 + tooltipSize.y / 2 + margin.y);
                break;
            case TooltipPosition.Below:
                adjustment = new Vector2(margin.x, -(targetUIElement.rect.height / 2 + tooltipSize.y / 2 + margin.y));
                break;
            case TooltipPosition.Left:
                adjustment = new Vector2(-(targetUIElement.rect.width / 2 + tooltipSize.x / 2 + margin.x), margin.y);
                break;
            case TooltipPosition.Right:
                adjustment = new Vector2(targetUIElement.rect.width / 2 + tooltipSize.x / 2 + margin.x, margin.y);
                break;
            case TooltipPosition.Center:
                adjustment = new Vector2(margin.x, margin.y); // Center might not use margin, but added for consistency
                break;
        }

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, canvas.worldCamera, out Vector2 localPoint);
            adjustment = canvasRectTransform.TransformVector(adjustment);
            return localPoint + adjustment;
        }
        else
        {
            return screenPoint + adjustment;
        }
    }
}
