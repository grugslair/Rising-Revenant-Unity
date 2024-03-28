using Dojo; using Dojo.Starknet; using Dojo.Torii;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementTrade : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("trade_id")]
    public FieldElement tradeId;

    [ModelField("trade_type")]
    public byte tradeType;

    [ModelField("seller")]
    public FieldElement sellerAddress;

    [ModelField("buyer")]
    public FieldElement buyerAddress;

    [ModelField("price")]
    public FieldElement price;

    [ModelField("offer")]
    public UInt32 offer;

    [ModelField("status")]
    public byte status;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Checking ReinforcementTrade");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"ReinforcementTrade was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"ReinforcementTrade was spared {gameId.Hex()}");
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
