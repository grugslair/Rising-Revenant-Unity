using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventConfirmPageBehaviour : Menu
{
    public GameObject parentPrefab;
    public GameObject elementPrefab;
    public GameObject sortingButton;
    public GameObject sortingMenu;

    public RawImage background;
    public List<Texture2D> picturesOfBackground = new List<Texture2D>();

    private bool toggleSortingMenu = false;

    private bool hideOwnOutposts = false;
    private bool hideOthersOutposts = false;

    private void OnEnable()
    {
        LoadLastEventData();
    }

    //possible call this when a new event hits form the manager if its enabled
    public void LoadLastEventData()
    {
        foreach (Transform child in parentPrefab.transform)
        {
            Destroy(child.gameObject);
        }

        background.texture = picturesOfBackground[(int)DojoEntitiesDataManager.currentWorldEvent.eventType];

        foreach (var outpost in DojoEntitiesDataManager.outpostDictInstance.Values)
        {
            if (outpost.isAttacked && outpost.life> 0)
            {
                 GameObject newItem = Instantiate(elementPrefab);

                 newItem.transform.SetParent(parentPrefab.transform, false);

                 newItem.GetComponent<EventPageDataContainerBehaviour>().Initialize(outpost.position);   
            }
        }
    }

    private void Update()
    {
        
    }

    public void ToggleSortingMenu()
    {
        toggleSortingMenu = !toggleSortingMenu;
        StartCoroutine(ToggleSortingMenuCoroutine(toggleSortingMenu));
    }

    private IEnumerator ToggleSortingMenuCoroutine(bool activate)
    {
        float targetScale = activate ? 0.9f : 1.0f;
        sortingMenu.SetActive(activate);

        Vector3 initialScale = sortingButton.transform.localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
        float duration = 0.2f; 
        float time = 0;

        while (time < duration)
        {
            sortingButton.transform.localScale = Vector3.Lerp(initialScale, targetScaleVector, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sortingButton.transform.localScale = targetScaleVector;
    }

    public void ToggleHideOwnValue()
    {
        hideOwnOutposts = !hideOwnOutposts;
        SortResults();
    }

    public void ToggleHideOthersValue()
    {
        hideOthersOutposts = !hideOthersOutposts;
        SortResults();
    }

    public void SortResults()
    {
        foreach (Transform child in parentPrefab.transform)
        {
            var comp = child.GetComponent<EventPageDataContainerBehaviour>();

            if (comp.outpost.ownerAddress.Hex() == DojoEntitiesDataManager.currentAccount.Address.Hex()) 
            {
                if (hideOwnOutposts)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
            else
            {
                if (hideOthersOutposts)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}
