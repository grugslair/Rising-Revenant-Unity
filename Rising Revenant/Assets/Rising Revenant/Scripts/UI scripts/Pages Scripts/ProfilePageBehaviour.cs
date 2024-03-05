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

    public int phase;

    private void OnEnable()
    {
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
    }

    private void OnDisable()
    {
        foreach (Transform child in parentObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
