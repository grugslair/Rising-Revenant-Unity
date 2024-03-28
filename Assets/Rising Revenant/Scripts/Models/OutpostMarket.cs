using Dojo; 
using Dojo.Starknet; 
using Dojo.Torii;
using System;
using UnityEngine;

public class OutpostMarket : ModelInstance
{

    [ModelField("game_id")]
    public FieldElement gameId;

    [ModelField("price")]
    public RisingRevenantUtils.U256 pricePerOutpost;

    [ModelField("available")]
    public UInt32 maxAmountOfOutposts;

    void Start()
    {
        Debug.Log("checking OutpostMarket");

        if (DojoEntitiesDataManager.currentGameId.Hex() != gameId.Hex())
        {
            Debug.Log($"OutpostMarket was destroyed {gameId.Hex()}");
            //Destroy(gameObject); return;
        }
        else
        {
            Debug.Log($"OutpostMarket was spared {gameId.Hex()}");
            DojoEntitiesDataManager.outpostMarketData = this;

            if (UiEntitiesReferenceManager.topBarUiElement != null)
            {
                UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
            }
        }



    }

    public override void OnUpdate(Model model)
    {
        Debug.Log("\nOutpostMarket");
        Debug.Log("low: " + pricePerOutpost.low.ToString() + "|| high: " + pricePerOutpost.high.ToString());

        base.OnUpdate(model);

        if (UiEntitiesReferenceManager.topBarUiElement != null)
        {
            UiEntitiesReferenceManager.topBarUiElement.ChangeInGameEntCounter();
        }
    }
}
