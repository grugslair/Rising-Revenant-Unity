using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterUiElement : MonoBehaviour
{
    [SerializeField]
    private RawImage minusIcons;
    [SerializeField]
    private RawImage plusIcons;
    [SerializeField]
    private TMP_Text valueText;

    public int currentValue = 1;
    public int additionValue = 1;

    public int lowestValue = 0;
    public int highestValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        currentValue = 1;
        valueText.text = currentValue.ToString();
    }

    
    // why two functions tho?
    public void AddToCounter()
    {
        currentValue++;

        if (highestValue < currentValue) {
            highestValue = currentValue;
        }
        valueText.text = currentValue.ToString();

    }

    public void RemoveFromCounter()
    {
        currentValue--;

        if (currentValue < lowestValue) {
            currentValue = lowestValue;
        }

        valueText.text = currentValue.ToString();
    }
}
