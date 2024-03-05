using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private GameObject currentTooltip;
    private RectTransform canvasRectTransform;

    private bool liveDeltaMove = false;
    private RectTransform currentTargetUIElement;
    private TooltipPosition currentPosition;
    private float currentMargin;


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

    public void ShowTooltip(GameObject tooltipPrefab, string message, RectTransform targetUIElement, TooltipPosition position, float margin, bool liveDeltaMove)
    {
        this.liveDeltaMove = liveDeltaMove;
        this.currentTargetUIElement = targetUIElement;
        this.currentPosition = position;
        this.currentMargin = margin;

        if (currentTooltip != null) Destroy(currentTooltip);

        // Use the provided prefab to instantiate the tooltip
        currentTooltip = Instantiate(tooltipPrefab, canvasRectTransform);
        currentTooltip.GetComponentInChildren<TMP_Text>().text = message;

        UpdateTooltipPosition(); // Set initial position
    }

    private void UpdateTooltipPosition()
    {
        if (currentTooltip == null || currentTargetUIElement == null) return;

        Vector2 targetPosition = GetTooltipPosition(currentTargetUIElement, currentPosition, currentMargin);
        currentTooltip.GetComponent<RectTransform>().anchoredPosition = targetPosition;
    }

    public void HideTooltip()
    {
        if (currentTooltip != null) Destroy(currentTooltip);
    }

    private void Update()
    {
        if (liveDeltaMove)
        {
            UpdateTooltipPosition();
        }
    }

    private Vector2 GetTooltipPosition(RectTransform targetUIElement, TooltipPosition position, float margin)
    {
        Canvas canvas = targetUIElement.GetComponentInParent<Canvas>();
        Vector3[] corners = new Vector3[4];
        targetUIElement.GetWorldCorners(corners);
        Vector3 targetPosition = Vector3.zero;

        // Calculate the midpoint of the target UI element
        foreach (var corner in corners)
        {
            targetPosition += corner;
        }
        targetPosition /= corners.Length;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, targetPosition);

        // No need for screen space adjustment here as we directly work with local position
        Vector2 tooltipSize = currentTooltip.GetComponent<RectTransform>().sizeDelta;
        Vector2 adjustment = Vector2.zero;

        switch (position)
        {
            case TooltipPosition.Above:
                adjustment = new Vector2(0, targetUIElement.rect.height / 2 + tooltipSize.y / 2 + margin);
                break;
            case TooltipPosition.Below:
                adjustment = new Vector2(0, -(targetUIElement.rect.height / 2 + tooltipSize.y / 2 + margin));
                break;
            case TooltipPosition.Left:
                adjustment = new Vector2(-(targetUIElement.rect.width / 2 + tooltipSize.x / 2 + margin), 0);
                break;
            case TooltipPosition.Right:
                adjustment = new Vector2(targetUIElement.rect.width / 2 + tooltipSize.x / 2 + margin, 0);
                break;
            // Center position does not use margin but included for completeness
            case TooltipPosition.Center:
                break;
        }

        // If your canvas is set to Screen Space - Overlay, and you're directly manipulating RectTransforms
        // Convert the screen position back to local position within the canvas
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out Vector2 localPoint);
            adjustment = canvas.GetComponent<RectTransform>().TransformVector(adjustment); // Convert adjustment vector to local space if needed
            return localPoint + adjustment;
        }
        else
        {
            return screenPoint + adjustment; // For World Space Canvas, direct addition should suffice
        }
    }
}
