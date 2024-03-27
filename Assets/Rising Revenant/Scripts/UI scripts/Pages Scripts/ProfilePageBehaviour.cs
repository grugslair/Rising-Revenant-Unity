using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePageBehaviour : Menu
{
    [SerializeField]
    private GameObject UiPrefabListItem;
    [SerializeField]
    private GameObject UiPrefabDiv;

    [SerializeField]
    private GameObject parentObject;

    public RisingRevenantUtils.Vec2? currentlySelectedOutpost = null;

    [SerializeField]
    private GameObject profileStandardView;
    [SerializeField]
    private GameObject profileReinforcementTypeView;

    [SerializeField]
    private GameObject tradeButton;

    public int phase;

    private void Awake()
    {
        UiEntitiesReferenceManager.profilePageBehaviour = this;
    }

    private void OnEnable()
    {
        if (phase == 1)
        {
            tradeButton.SetActive(false);
        }

        for (int i = 0; i < DojoEntitiesDataManager.ownOutpostIndex.Count; i++)
        {
            var item = DojoEntitiesDataManager.ownOutpostIndex[i];
            GameObject newItem = Instantiate(UiPrefabListItem);
            newItem.transform.SetParent(parentObject.transform, false);
            newItem.GetComponent<OutpostDataProfilePageListElement>().InitiateData(item, phase);

            if (i < DojoEntitiesDataManager.ownOutpostIndex.Count - 1)
            {
                GameObject div = Instantiate(UiPrefabDiv);
                div.transform.SetParent(parentObject.transform, false);
            }
        }

        currentlySelectedOutpost = default;
    }

    private void Update()
    {
        if (currentlySelectedOutpost != null && !profileReinforcementTypeView.activeSelf)
        {
            profileReinforcementTypeView.SetActive(true);
            profileStandardView.SetActive(false);
        }
        else if (currentlySelectedOutpost == null && !profileStandardView.activeSelf)
        {
            profileReinforcementTypeView.SetActive(false);
            profileStandardView.SetActive(true);
        }
    }

    public void SetOutpostToNull()
    {
        currentlySelectedOutpost = null;
    }

    private void OnDisable()
    {
        foreach (Transform child in parentObject.transform)
        {
            Destroy(child.gameObject);
        }

        currentlySelectedOutpost = default;
    }
}
