using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevenantJurnalComponentBehaviour : MonoBehaviour
{
    public TMP_Text eventNumText;
    public TMP_Text eventDataText;

    public GameObject parentList;
    public GameObject parentListPrefab;

    private void Awake()
    {
        UiEntitiesReferenceManager.revJournalCompBehaviour = this;
    }

    // we want this to be called from the eventManager, no need for subs
    //at start this should be emptied or maybe just by defaulr in editr empty it

    public void HandleWorldEventAdded(CurrentWorldEvent worldEvent)
    {
        //eventNumText.text = $"Event Data #{RisingRevenantUtils.FieldElementToInt(worldEvent)}";
        //on call queyr to tot count?
        eventNumText.text = $"Event Data {DojoEntitiesDataManager.worldEventDictInstance.Count -1}";
        eventDataText.text = $"Radius: {worldEvent.radius} km\n" +
                             $"Position: X:{worldEvent.position.x} || Y:{worldEvent.position.y}\n" +
                             $"Next Attack: IDK";

        //kill all children
        foreach (Transform child in parentList.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in DojoEntitiesDataManager.ownOutpostIndex)
        {
            var outpost = DojoEntitiesDataManager.outpostDictInstance[item];

            if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(worldEvent.position.x, worldEvent.position.y), worldEvent.radius, new Vector2(outpost.position.x, outpost.position.y)))
            {
                if (outpost.life > 0)
                {
                    GameObject newItem = Instantiate(parentListPrefab);

                    newItem.transform.SetParent(parentList.transform, false);

                    newItem.GetComponentInChildren<TMP_Text>().text = $"Outpost ID: {RisingRevenantUtils.CantonPair((int)outpost.position.x, (int)outpost.position.y)}  ||  X:{outpost.position.x}, Y:{outpost.position.y}";
                }
            }
        }
    }
}
