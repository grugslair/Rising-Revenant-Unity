using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//HERE this should be satrted with no data as right now its pre populated

public class RevenantJurnalPageBehaviour : Menu
{
    public TMP_Text eventNumText;
    public TMP_Text eventDataTypeText;
    public TMP_Text eventDataCoordText;
    public TMP_Text eventDataRadiusText;

    public GameObject parentList;
    public GameObject parentListPrefab;

    private WorldEvent currentWorldEvent = null;

    private int currentlySelectedIndex = 0;

    void OnEnable()
    {
        currentlySelectedIndex = (int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount;

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

    public void ChangeEvent(int index)
    {
        if (currentlySelectedIndex - index <= 0) { return; }
        if (currentlySelectedIndex + index > (int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount) { return; }

        currentlySelectedIndex += index;
    }

    private void HandleWorldEventAdded(WorldEvent worldEvent)
    {
        if (DojoEntitiesDataManager.gameEntityCounterInstance.eventCount > 0)
        {
            Debug.Log(DojoEntitiesDataManager.worldEventDictInstance[(int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount]);
            if (currentWorldEvent != DojoEntitiesDataManager.worldEventDictInstance[(int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount])
            {
                currentWorldEvent = DojoEntitiesDataManager.worldEventDictInstance[(int)DojoEntitiesDataManager.gameEntityCounterInstance.eventCount];

                eventNumText.text = $"Event {RisingRevenantUtils.FieldElementToInt(currentWorldEvent.entityId)}/{DojoEntitiesDataManager.gameEntityCounterInstance.eventCount}";

                eventDataRadiusText.text = $"Radius\n{currentWorldEvent.radius}km";
                eventDataCoordText.text = $"Position\nX:{currentWorldEvent.xPosition} | |X:{currentWorldEvent.yPosition}";
                eventDataTypeText.text = $"Type\nNull";

                //kill all children
                foreach (Transform child in parentList.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (var item in DojoEntitiesDataManager.outpostDictInstance)
                {
                    var outpost = item.Value;

                    if (RisingRevenantUtils.IsPointInsideCircle(new Vector2(currentWorldEvent.xPosition, currentWorldEvent.yPosition), currentWorldEvent.radius, new Vector2(outpost.xPosition, outpost.yPosition)))
                    {
                        GameObject newItem = Instantiate(parentListPrefab);
                        newItem.transform.SetParent(parentList.transform, false);
                        var comp = newItem.GetComponent<RevenantJournalPageListElement>();

                        var id = RisingRevenantUtils.FieldElementToInt(outpost.entityId);

                        comp.OutpostCoordsText.text = $"X:{outpost.xPosition}\nY:{outpost.yPosition}";
                        comp.OutpostNameText.text = $"Owner: {RisingRevenantUtils.GetFullRevenantName(id)}";
                        comp.OutpostIdText.text = $"Outpost Id: {id}";
                    }
                }
            }
        }
    }
}
