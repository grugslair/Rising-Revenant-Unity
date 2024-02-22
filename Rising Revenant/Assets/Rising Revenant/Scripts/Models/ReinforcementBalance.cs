using Dojo;
using Dojo.Starknet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementBalance : ModelInstance
{
    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("target_price")]
    public FieldElement targetPrice;
    [ModelField("start_timestamp")]
    public UInt64 startTimestamp;
    [ModelField("count")]
    public UInt32 count;
}
