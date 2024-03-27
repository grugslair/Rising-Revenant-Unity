using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DoubleHandSlider : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform handleMin;
    public RectTransform handleMax;
    public RectTransform sliderTrack;

    public float minValue = 1f;
    public float maxValue = 20f;

    public float currentMinValue = 1f;
    public float currentMaxValue = 20f;

    public bool useWholeNumbers = true;
    public int decimalPlaces = 2; 

    private bool draggingMinHandle = false;
    private bool draggingMaxHandle = false;

    public delegate void ValueChangedDelegate(float newMinValue, float newMaxValue);
    public ValueChangedDelegate onValueChanged;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.pressPosition.x - handleMin.position.x) < Mathf.Abs(eventData.pressPosition.x - handleMax.position.x))
        {
            draggingMinHandle = true;
        }
        else
        {
            draggingMaxHandle = true;
        }
    }

    private void Start()
    {
        currentMaxValue = maxValue;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingMinHandle)
        {
            MoveHandle(eventData.position, true);
        }
        else if (draggingMaxHandle)
        {
            MoveHandle(eventData.position, false);
        }
    }

    private void MoveHandle(Vector2 position, bool isMinHandle)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderTrack, position, null, out localPoint);

        float percent = Mathf.Clamp01((localPoint.x - sliderTrack.rect.min.x) / sliderTrack.rect.width );
        float value = percent * (maxValue - minValue) + minValue;

        value = AdjustValuePrecision(value);

        if (isMinHandle)
        {
            float newMinValue = Mathf.Clamp(value, minValue, currentMaxValue);

            if (  newMinValue >= currentMaxValue) {
            }
            else if (newMinValue != currentMinValue)
            {
                currentMinValue = newMinValue;
                handleMin.anchoredPosition = new Vector2((percent * sliderTrack.rect.width) - sliderTrack.rect.width/2, handleMin.anchoredPosition.y);
            }
        }
        else
        {
            float newMaxValue = Mathf.Clamp(value, currentMinValue, maxValue);

            if (newMaxValue <= currentMinValue)
            {
            }
            else if (newMaxValue != currentMaxValue)
            {
                currentMaxValue = newMaxValue;
                handleMax.anchoredPosition = new Vector2(percent * sliderTrack.rect.width - sliderTrack.rect.width / 2, handleMax.anchoredPosition.y);
            }
        }
    }

    private float AdjustValuePrecision(float value)
    {
        if (useWholeNumbers)
        {
            return Mathf.Round(value);
        }
        else
        {
            return (float)System.Math.Round(value, decimalPlaces);
        }
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            draggingMinHandle = false;
            draggingMaxHandle = false;
        }
    }
}
