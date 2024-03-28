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

    [ModelField("decay_constant_mag")]
    public FieldElement decayConstant;

    [ModelField("max_sellable")]
    public UInt32 maxSellable;

    [ModelField("time_scale_mag")]
    public FieldElement timeScaleMag;

    [ModelField("start_block_number")]
    public UInt64 startBlockNumber;

    [ModelField("sold")]
    public UInt32 sold;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Checking ReinforcementMarket");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"ReinforcementMarket was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"ReinforcementMarket was spared {gameId.Hex()}");
        }
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
