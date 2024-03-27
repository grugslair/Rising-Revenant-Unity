using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SideMenuContentSizer : MonoBehaviour
{
    [SerializeField] private RawImage[] icons;

    public float takeOutFromSize = 5f;
    public float scaleWHStandard = 75f;

    public int emptyAfter = 99;

    public int lastSelected = -1;

    public float animDuration = 0.2f;

    public void CallToSideBar(int indexClickedOn)
    {
        if (emptyAfter < indexClickedOn)
        {
            return;
        }

        if (lastSelected == indexClickedOn)
        {
            ResetSizes();
            lastSelected = -1;
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 targetSize = i == indexClickedOn ? new Vector2(scaleWHStandard + takeOutFromSize * 4, scaleWHStandard + takeOutFromSize * 4) : new Vector2(scaleWHStandard - takeOutFromSize, scaleWHStandard - takeOutFromSize);
                StartCoroutine(ChangeSizeOverTime(icons[i], targetSize, animDuration)); // 1 second duration
            }
            lastSelected = indexClickedOn;
        }
    }

    public void ResetSizes()
    {
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(ChangeSizeOverTime(icons[i], new Vector2(scaleWHStandard, scaleWHStandard), animDuration)); // 1 second duration
        }
    }

    IEnumerator ChangeSizeOverTime(RawImage icon, Vector2 targetSize, float duration)
    {
        float time = 0;
        Vector2 startSize = icon.rectTransform.sizeDelta;

        while (time < duration)
        {
            icon.rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the target size is set after the interpolation
        icon.rectTransform.sizeDelta = targetSize;
    }
}
