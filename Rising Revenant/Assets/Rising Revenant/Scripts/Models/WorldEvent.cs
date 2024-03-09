using Dojo; using Dojo.Starknet; using Dojo.Torii;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldEvent : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("event_id")]
    public FieldElement eventId;

    // Assuming Position is a known type that needs conversion or handling
    [ModelField("position")]
    public RisingRevenantUtils.Vec2 position;

    [ModelField("radius")]
    public UInt32 radius;

    [ModelField("event_type")]
    public RisingRevenantUtils.EventType eventType;

    [ModelField("number")]
    public UInt32 number;

    [ModelField("block_number")]
    public UInt64 blockNumber;

    [ModelField("previous_event")]
    public FieldElement previousEvent;

    [ModelField("next_event")]
    public FieldElement nextEvent;



    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("laoding in event id " + RisingRevenantUtils.FieldElementToInt(eventId));
        DojoEntitiesDataManager.worldEventDictInstance.Add(number, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnUpdate(Model model)
    {
        base.OnUpdate(model);
    }
}
