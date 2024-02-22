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
        foreach (var item in DojoEntitiesDataManager.ownOutpostIndex)
        {
            GameObject newItem = Instantiate(UiPrefabListItem);
            newItem.transform.SetParent(parentObject.transform, false);
            newItem.GetComponent<OutpostDataProfilePageListElement>().InitiateData(item,phase );

            GameObject div = Instantiate(UiPrefabDiv);
            div.transform.SetParent(parentObject.transform, false);
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
