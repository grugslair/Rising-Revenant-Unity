using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class ButtonInteractionScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D pictureOfPointer; 

    public TMP_Text text;
    public Image backgroundColor;
    public Image borderImage;

    public Color startBackGroundColor;
    public Color endBackGroundColor;

    public Color startTextColor;
    public Color endTextColor;

    public Color startImageColor;
    public Color endImageColor;

    public float transitionSpeed = 1.0f;

    public bool disabled = false; 
    //disabled only works on initial start

    private Coroutine backgroundTransitionCoroutine;
    private Coroutine textTransitionCoroutine;
    private Coroutine borderImageTransitionCoroutine;

    // the issue is that the button will not stay active

    void Start()
    {
        UpdateDisabledState(); 
    }

    //on disable everythign should go back 
    private void OnDisable()
    {
        ResetColors(); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (disabled) return;

        StartTransition(endBackGroundColor, endTextColor, endImageColor);
        //Cursor.SetCursor(pictureOfPointer, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (disabled) return; 

        StartTransition(startBackGroundColor, startTextColor, startImageColor);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
    }

    void StartTransition(Color targetBackgroundColor, Color targetTextColor, Color targetImageColor)
    {
        if (disabled) return; 

        if (backgroundTransitionCoroutine != null) StopCoroutine(backgroundTransitionCoroutine);
        backgroundTransitionCoroutine = StartCoroutine(TransitionColor(backgroundColor, targetBackgroundColor, transitionSpeed));

        if (textTransitionCoroutine != null) StopCoroutine(textTransitionCoroutine);
        textTransitionCoroutine = StartCoroutine(TransitionColor(text, targetTextColor, transitionSpeed));

        if (borderImageTransitionCoroutine != null) StopCoroutine(borderImageTransitionCoroutine);
        borderImageTransitionCoroutine = StartCoroutine(TransitionColor(borderImage, targetImageColor, transitionSpeed));
    }

    IEnumerator TransitionColor(Graphic targetGraphic, Color targetColor, float duration)
    {
        Color startColor = targetGraphic.color;
        float time = 0;

        while (time < duration)
        {
            targetGraphic.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        targetGraphic.color = targetColor;
    }

    private void UpdateDisabledState()
    {
        if (disabled)
        {
            SetOpacity(startBackGroundColor, startTextColor, startImageColor, 0.5f);
        }
        else
        {
            ResetColors();
        }
    }

    private void SetOpacity(Color backgroundColor, Color textColor, Color imageColor, float opacity)
    {
        this.backgroundColor.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, opacity);
        this.text.color = new Color(textColor.r, textColor.g, textColor.b, opacity);
        this.borderImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, opacity);
    }

    private void ResetColors()
    {
        backgroundColor.color = startBackGroundColor;
        text.color = startTextColor;
        borderImage.color = startImageColor;
    }
}