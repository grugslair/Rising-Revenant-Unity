using Dojo; 
using Dojo.Starknet;
using Dojo.Torii;
using System;
using UnityEngine;

public class CurrentWorldEvent : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("event_id")]
    public FieldElement eventId;

    [ModelField("position")]
    public RisingRevenantUtils.Vec2 position;

    [ModelField("radius")]
    public UInt32 radius;

    [ModelField("number")]
    public UInt32 number;

    [ModelField("block_number")]
    public UInt64 blockNumber;

    [ModelField("previous_event")]
    public FieldElement previousEvent;

    void Start()
    {
        DojoEntitiesDataManager.currentWorldEvent = this;

        if(UiEntitiesReferenceManager.worldEventManager != null)
        {
            if (UiEntitiesReferenceManager.worldEventManager.transform.gameObject.activeSelf == false)
            {
                UiEntitiesReferenceManager.worldEventManager.transform.gameObject.SetActive(true);
            }

            UiEntitiesReferenceManager.worldEventManager.LoadLastWorldEventData(this);
        }
        else
        {
            Debug.Log(UiEntitiesReferenceManager.worldEventManager);
        }
    }

    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);

        if (UiEntitiesReferenceManager.worldEventManager != null)
        {
            Debug.Log("this is here");
            if (UiEntitiesReferenceManager.worldEventManager.transform.gameObject.activeSelf == false)
            {
                Debug.Log("the world event manager is false");
                UiEntitiesReferenceManager.worldEventManager.transform.gameObject.SetActive(true);
            }

            UiEntitiesReferenceManager.worldEventManager.LoadLastWorldEventData(this);
        }

        //OnValueChange?.Invoke(RisingRevenantUtils.FieldElementToInt(eventId));
    }
}
