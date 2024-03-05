 using Dojo; using Dojo.Starknet; using Dojo.Torii; 
using System.Collections.Generic;
using UnityEngine;

public class OutpostTrade : ModelInstance
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

    // Assuming Position is a known type that needs conversion or handling
    [ModelField("offer")]
    public RisingRevenantUtils.Vec2 offer;

    [ModelField("status")]
    public byte status;


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
