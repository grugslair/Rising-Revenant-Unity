using Dojo;
using Dojo.Starknet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEvent : ModelInstance
{
    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("entity_id")]
    public FieldElement entityId;
    [ModelField("x")]
    public UInt32 xPosition;
    [ModelField("y")]
    public UInt32 yPosition;
    [ModelField("radius")]
    public UInt32 radius;
    [ModelField("destroy_count")]
    public UInt32 destroydOutpostsCount;
    [ModelField("block_number")]
    public UInt64 blockCountStartedAt;

    private void Start()
    {
        Debug.Log("is this getting called");
        DojoEntitiesDataManager.AddEvent(RisingRevenantUtils.FieldElementToInt(entityId), this);
    }
}
