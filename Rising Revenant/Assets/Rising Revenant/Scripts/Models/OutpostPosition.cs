using Dojo;
using Dojo.Starknet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostPosition : ModelInstance
{
    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("y")]
    public UInt32 x;
    [ModelField("x")]
    public UInt32 y;
    [ModelField("entity_id")]
    public FieldElement entityId;
}
