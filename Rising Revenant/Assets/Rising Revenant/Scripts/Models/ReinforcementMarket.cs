using Dojo; using Dojo.Starknet; using Dojo.Torii;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementMarket : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("target_price")]
    public FieldElement targetPrice;

    [ModelField("start_block_number")]
    public UInt64 startTimestamp;

    [ModelField("decay_constant")]
    public FieldElement decayConstant;

    [ModelField("max_sellable")]
    public UInt32 maxSellable;

    [ModelField("count")]
    public UInt32 count;


    // Start is called before the first frame update
    void Start()
    {
        
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
