using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevenantJournalComponentBehaviour : MonoBehaviour
{
    public TMP_Text eventNumText;
    public TMP_Text eventDataText;

    public GameObject parentList;
    public GameObject parentListPrefab;

    private WorldEvent currentWorldEvent = null;

    void OnEnable()
    {
        DojoEntitiesDataManager.OnWorldEventAdded += HandleWorldEventAdded;
        var worldEvent = DojoEntitiesDataManager.GetLatestEvent();

        if (worldEvent != null)
        {
            HandleWorldEventAdded(worldEvent);
        }
    }

    void OnDisable()
    {
        DojoEntitiesDataManager.OnWorldEventAdded -= HandleWorldEventAdded;
    }

    private void HandleWorldEventAdded(WorldEvent worldEvent)
    {
        if (DojoEntitiesDataManager.gameEntityCounterInstance.eventCount > 0)
        {
            if (currentWorldEvent != DojoEntitiesDataManager.worldEventDictInstance[(int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount])
            {
                currentWorldEvent = DojoEntitiesDataManager.worldEventDictInstance[(int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount];

                eventNumText.text = $"Event Data #{RisingRevenantUtils.FieldElementToInt(currentWorldEvent.entityId)}";

                eventDataText.text = $"Radius: {currentWorldEvent.radius}km\n" +
                                     $"Position: X:{currentWorldEvent.xPosition} || Y:{currentWorldEvent.yPosition}\n" +
                                     $"Next Attack IDK";

                //kill all children
                foreach (Transform child in parentList.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (var item in DojoEntitiesDataManager.ownOutpostIndex)
                {
                    var outpost = DojoEntitiesDataManager.outpostDictInstance[(int)item];

                    if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(currentWorldEvent.xPosition, currentWorldEvent.yPosition),currentWorldEvent.radius, new Vector2(outpost.xPosition,outpost.yPosition)))
                    {
                        if (outpost.lifes > 0)
                        {
                            GameObject newItem = Instantiate(parentListPrefab);

                            newItem.transform.SetParent(parentList.transform, false);

                            newItem.GetComponentInChildren<TMP_Text>().text = $"Outpost ID: {RisingRevenantUtils.FieldElementToInt(outpost.entityId)}  ||  X:{outpost.xPosition}, Y:{outpost.yPosition}";
                        }
                    }
                }
            }
        }
        else
        {
            eventNumText.text = $"No Event Yet ";

            eventDataText.text = $"Radius: ...\n" +
                                 $"Position: X:... || Y:..\n" +
                                 $"Next Attack Imminent";

            foreach (Transform child in parentList.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
