using Dojo;
using Dojo.Starknet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeRevenant : ModelInstance
{
    [ModelField("game_id")]
    public UInt32 gameId;
    [ModelField("entity_id")]
    public UInt32 entityId;
    [ModelField("seller")]
    public FieldElement sellerAddress;
    [ModelField("price")]
    public FieldElement priceOfTrade;
    [ModelField("revenant_id")]
    public FieldElement revenantIdSelling;
    [ModelField("outpost_id")]
    public FieldElement outpostIdSelling;
    [ModelField("buyer")]
    public FieldElement buyerAddress;
    [ModelField("status")]
    public UInt32 statusOfTrade;
}
